using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AuthService.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtFacade _jwtFacade;

        public IdentityService(
            UserManager<IdentityUser> userManager,
            JwtFacade jwtFacade
        )
        {
            _userManager = userManager;
            _jwtFacade = jwtFacade;
        }

        public async Task<AuthResult> RegisterAsync(RegisterRequest request,
            CancellationToken cancellationToken = default)
        {
            var username = request.Username;
            var email = request.Email;
            var password = request.Password;

            // Check email exists
            var exists = await _userManager.FindByEmailAsync(email);
            if (exists is null)
            {
                return new AuthResult
                {
                    Errors = new List<string> {"Email have been used!"},
                    Succeeded = false
                };
            }

            var newUser = new IdentityUser
            {
                UserName = username,
                Email = email,
            };

            var createdResult = await _userManager.CreateAsync(newUser, password);
            if (!createdResult.Succeeded)
            {
                return new AuthResult
                {
                    Errors = createdResult.Errors.Select(error => $"{error.Code}: {error.Description}"),
                    Succeeded = false
                };
            }

            return new AuthResult
            {
                Succeeded = createdResult.Succeeded
            };
        }

        public async Task<AuthResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
        {
            var invalidResult = new AuthResult
            {
                Errors = new List<string> {"Invalid credentials"},
                Succeeded = false
            };

            var email = request.Email;
            var password = request.Password;

            // Find user
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) return invalidResult;

            // Verify password
            var checkPassword = await _userManager.CheckPasswordAsync(user, password);
            if (!checkPassword) return invalidResult;

            // Generate token
            var token = _jwtFacade.GenerateToken(CreateUserClaims(user));
            return new AuthResult
            {
                Succeeded = true,
                Token = token
            };
        }

        public IEnumerable<Claim> CreateUserClaims(IdentityUser user)
        {
            return new List<Claim>
            {
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(JwtRegisteredClaimNames.Sub, user.Email),
                new(JwtRegisteredClaimNames.Name, user.UserName),
            };
        }
    }
}
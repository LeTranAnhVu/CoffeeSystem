using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AuthService.Contracts;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Services
{
    public interface IIdentityService
    {
        public Task<AuthResult> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
        public Task<AuthResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
        public AuthResult ValidateToken(ClaimsPrincipal user);
    }

    public class LoginRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class RegisterRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class AuthResult
    {
        public IEnumerable<string> Errors { get; set; }
        public bool Succeeded { get; set; }
        public string Token { get; set; }

        public UserReadContract User { get; set; }
    }
}
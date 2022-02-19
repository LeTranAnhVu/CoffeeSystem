using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Services
{
    public interface IIdentityService
    {
        public Task<AuthResult> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
        public Task<AuthResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
        public AuthResult ValidateToken(ClaimsPrincipal user);
    }
}
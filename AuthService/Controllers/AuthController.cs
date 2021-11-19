using System.Threading;
using System.Threading.Tasks;
using AuthService.Contracts;
using AuthService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IIdentityService _identityService;

        public AuthController(ILogger<AuthController> logger, IIdentityService identityService)
        {
            _logger = logger;
            _identityService = identityService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponseContract>> Register(RegisterRequestContract registerRequestContract, CancellationToken cancellationToken)
        {
            var registerRequest = new RegisterRequest
            {
                Username = registerRequestContract.Username,
                Email = registerRequestContract.Email,
                Password = registerRequestContract.Password
            };

            var result = await _identityService.RegisterAsync(registerRequest, cancellationToken);
            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    Message = string.Join("\n", result.Errors)
                });
            }

            return new RegisterResponseContract
            {
                Message = "Register successful"
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseContract>> Login(LoginRequestContract loginRequestContract, CancellationToken cancellationToken)
        {
            var loginRequest = new LoginRequest()
            {
                Email = loginRequestContract.Email,
                Password = loginRequestContract.Password
            };

            var loginResult = await _identityService.LoginAsync(loginRequest, cancellationToken);
            if (!loginResult.Succeeded)
            {
                return BadRequest(new
                {
                    Message = string.Join("\n", loginResult.Errors)
                });
            }

            return new LoginResponseContract
            {
                AccessToken = loginResult.Token
            };
        }
    }
}
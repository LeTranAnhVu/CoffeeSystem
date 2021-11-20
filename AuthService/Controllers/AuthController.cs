using AuthService.Dtos;
using AuthService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IIdentityService _identityService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthController(ILogger<AuthController> logger, IIdentityService identityService, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _identityService = identityService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponseDto>> Register(RegisterRequestDto registerRequestContract, CancellationToken cancellationToken)
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

            return new RegisterResponseDto
            {
                Message = "Register successful"
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto loginRequestContract, CancellationToken cancellationToken)
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

            return new LoginResponseDto
            {
                AccessToken = loginResult.Token
            };
        }

        [Authorize]
        [HttpGet("validateToken")]
        public ActionResult<AuthResult> ValidateToken(CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext.User;
            var result = _identityService.ValidateToken(user);
            return Ok(result);
        }
    }
}
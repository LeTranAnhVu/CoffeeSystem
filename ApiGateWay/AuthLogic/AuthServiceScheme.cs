using System.Security.Claims;
using System.Text.Encodings.Web;
using ApiGateWay.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace ApiGateWay.AuthLogic
{
    public static class AuthServiceScheme
    {
        public static readonly string DefaultName =  "Custom";
    }

    public class AuthServiceSchemeOptions : AuthenticationSchemeOptions
    {
    }

    public class AuthServiceHandler : AuthenticationHandler<AuthServiceSchemeOptions>
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthServiceHandler> _logger;

        public AuthServiceHandler(ILoggerFactory loggerFactory,IAuthService authService, IOptionsMonitor<AuthServiceSchemeOptions> options,UrlEncoder encoder, ISystemClock clock) : base(options, loggerFactory, encoder, clock)
        {
            _logger = loggerFactory.CreateLogger<AuthServiceHandler>();
            _authService = authService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            try
            {
                var endpoint = Context.GetEndpoint();
                if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
                {
                    return await Task.FromResult(AuthenticateResult.NoResult());
                }

                // Get the token
                var token = Context.Request.Headers.Authorization.ToString();
                if (string.IsNullOrWhiteSpace(token))
                {
                    return await Task.FromResult(AuthenticateResult.Fail("Unauthenticated"));
                }

                var tokenArr = token.Split(" ");
                var scheme = tokenArr[0];
                var accessToken = tokenArr[1];
                var authResult = await _authService.ValidateUser(accessToken, scheme);

                if (!authResult.Succeeded)
                {
                    return await Task.FromResult(AuthenticateResult.Fail("Unauthenticated"));
                }

                var authResultUser = authResult.User;
                var claims = new List<Claim>
                {
                    new (ClaimTypes.Email,authResultUser.Email),
                    new (ClaimTypes.Name,authResultUser.Username),
                };
                // generate claimsIdentity on the name of the class
                var claimsIdentity = new ClaimsIdentity(claims, Scheme.Name);

                // generate AuthenticationTicket from the Identity
                // and current authentication scheme
                var ticket = new AuthenticationTicket(new ClaimsPrincipal(claimsIdentity), Scheme.Name);
                return await Task.FromResult(AuthenticateResult.Success(ticket));
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Authentication fail Error is: {e.Message}");
                return await Task.FromResult(AuthenticateResult.Fail("Unauthenticated"));
            }

        }
    }
}
using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ProductService.AuthLogic
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
        public AuthServiceHandler(IOptionsMonitor<AuthServiceSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var endpoint = Context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
            {
                return await Task.FromResult(AuthenticateResult.NoResult());
            }
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1")
            };

            // Console.WriteLine(Request);

            // generate claimsIdentity on the name of the class
            var claimsIdentity = new ClaimsIdentity(claims, Scheme.Name);

            // generate AuthenticationTicket from the Identity
            // and current authentication scheme
            var ticket = new AuthenticationTicket(new ClaimsPrincipal(claimsIdentity), Scheme.Name);
            // return Task.FromResult(AuthenticateResult.Success(ticket));
            return await Task.FromResult(AuthenticateResult.Fail("fail"));
        }
    }
}
using System.Net.Http.Headers;
using ApiGateWay.Dtos;
using Microsoft.Extensions.Options;

namespace ApiGateWay.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly string _url;
    private readonly HttpClient _authHttpClient;

    public AuthService(IHttpClientFactory httpFactory, IOptions<AuthServiceSettings> authServiceSettings)
    {
        _url = authServiceSettings.Value.Url ?? throw new NullReferenceException();
        _authHttpClient = httpFactory.CreateClient();
        _authHttpClient.BaseAddress = new Uri(_url);
        _authHttpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        _authHttpClient.DefaultRequestHeaders.Add("ContentType", "application/json");
    }

    public async Task<AuthResponseDto> ValidateUser(string accessToken, string scheme, CancellationToken cancellationToken)
    {
        try
        {
            _authHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var authResult =
                await _authHttpClient.GetFromJsonAsync<AuthResponseDto>("/api/auth/validateToken",
                    cancellationToken: cancellationToken);
            if (authResult is not null)
            {
                return authResult;
            }

            return new AuthResponseDto()
            {
                Errors = new[] { "Authentication fail!" },
                Succeeded = false
            };
        }
        catch (Exception e)
        {
            return new AuthResponseDto()
            {
                Errors = new[] { "Authentication fail!" },
                Succeeded = false
            };
        }

    }
}
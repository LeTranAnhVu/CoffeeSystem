using AuthForServicesExtension.Dtos;

namespace AuthForServicesExtension.Services.AuthService;

public interface IAuthService
{
    public Task<AuthResponseDto> ValidateUser(string accessToken, string scheme = "Bearer",CancellationToken cancellationToken = default);
}
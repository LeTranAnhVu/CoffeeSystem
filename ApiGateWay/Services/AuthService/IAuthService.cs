using ApiGateWay.Dtos;

namespace ApiGateWay.Services;

public interface IAuthService
{
    public Task<AuthResponseDto> ValidateUser(string accessToken, string scheme = "Bearer",CancellationToken cancellationToken = default);
}
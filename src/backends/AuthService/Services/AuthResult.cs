using AuthService.Dtos;

namespace AuthService.Services;

public class AuthResult
{
    public IEnumerable<string> Errors { get; set; }
    public bool Succeeded { get; set; }
    public string Token { get; set; }
    public UserReadDto User { get; set; }
}
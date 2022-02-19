namespace AuthForServicesExtension.Dtos;

public class UserReadDto
{
    public string Email { get; set; }
    public string Username { get; set; }
}

public class AuthResponseDto
{
    public IEnumerable<string> Errors { get; set; }
    public bool Succeeded { get; set; }
    public string Token { get; set; }
    public UserReadDto User { get; set; }
}

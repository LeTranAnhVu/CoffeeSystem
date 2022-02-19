using System.ComponentModel.DataAnnotations;

namespace AuthService.Services;

public class LoginRequest
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace AuthService.Dtos
{
    public class LoginRequestDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class LoginResponseDto
    {
        public string AccessToken { get; set; }
    }
}
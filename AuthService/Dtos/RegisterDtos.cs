using System.ComponentModel.DataAnnotations;

namespace AuthService.Dtos
{
    public class RegisterRequestDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class RegisterResponseDto
    {
        public string Message { get; set; }
    }
}
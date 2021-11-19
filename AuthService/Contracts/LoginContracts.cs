using System.ComponentModel.DataAnnotations;

namespace AuthService.Contracts
{
    public class LoginRequestContract
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class LoginResponseContract
    {
        public string AccessToken { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace Auth.Service.Contracts
{
    public class RegisterRequestContract
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class RegisterResponseContract
    {
        public string Message { get; set; }
    }
}
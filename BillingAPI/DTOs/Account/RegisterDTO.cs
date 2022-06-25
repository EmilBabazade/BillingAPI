using System.ComponentModel.DataAnnotations;

namespace BillingAPI.DTOs.Account
{
    public class RegisterDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}

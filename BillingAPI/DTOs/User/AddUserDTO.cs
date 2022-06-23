using System.ComponentModel.DataAnnotations;

namespace BillingAPI.DTOs.User
{
    public class AddUserDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}

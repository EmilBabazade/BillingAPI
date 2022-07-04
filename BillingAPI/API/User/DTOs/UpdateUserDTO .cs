using System.ComponentModel.DataAnnotations;

namespace BillingAPI.API.User.DTOs
{
    public class UpdateUserDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}

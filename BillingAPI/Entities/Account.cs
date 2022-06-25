using System.ComponentModel.DataAnnotations;

namespace BillingAPI.Entities
{
    public class Account
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}

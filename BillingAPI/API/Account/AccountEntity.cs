using System.ComponentModel.DataAnnotations;

namespace BillingAPI.API.Account
{
    public class AccountEntity
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}

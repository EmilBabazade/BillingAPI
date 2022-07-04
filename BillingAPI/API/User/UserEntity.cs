using BillingAPI.API.Balance;
using BillingAPI.API.Order;
using BillingAPI.API.Payments;
using System.ComponentModel.DataAnnotations;

namespace BillingAPI.API.User
{
    public class UserEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastUpdatedAt { get; set; }
        public List<BalanceEntity> Balances { get; set; }
        public List<PaymentEntity> Payments { get; set; }
        public List<OrderEntity> Orders { get; set; }
    }
}

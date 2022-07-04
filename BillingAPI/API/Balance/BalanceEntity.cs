using BillingAPI.API.Payments;
using BillingAPI.API.User;
using System.ComponentModel.DataAnnotations;

namespace BillingAPI.API.Balance
{
    public class BalanceEntity
    {
        [Key]
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastUpdatedAt { get; set; }
        public int UserId { get; set; }
        public UserEntity User { get; set; }
        public int PaymentId { get; set; }
        public PaymentEntity Payment { get; set; }
    }
}

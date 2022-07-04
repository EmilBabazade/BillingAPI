using BillingAPI.API.Balance;
using BillingAPI.API.Gateway;
using BillingAPI.API.Order;
using BillingAPI.API.User;
using System.ComponentModel.DataAnnotations;

namespace BillingAPI.API.Payments
{
    public class PaymentEntity
    {
        [Key]
        public int Id { get; set; }
        public decimal Amount { get; set; } = 0;
        public string? Description { get; set; }
        public bool IsSuccessfull { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastUpdatedAt { get; set; }
        public BalanceEntity? Balance { get; set; }
        public int? GatewayId { get; set; }
        public GatewayEntity? Gateway { get; set; }
        public OrderEntity? Order { get; set; }
        public int UserId { get; set; }
        public UserEntity User { get; set; }
    }
}

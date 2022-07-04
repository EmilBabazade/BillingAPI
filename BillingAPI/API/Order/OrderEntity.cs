using BillingAPI.API.Gateway;
using BillingAPI.API.Payments;
using BillingAPI.API.User;
using System.ComponentModel.DataAnnotations;

namespace BillingAPI.API.Order
{
    public class OrderEntity
    {
        [Key]
        public int Id { get; set; }
        public string No { get; set; }
        public decimal PayableAmount { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastUpdatedAt { get; set; }
        public int GatewayId { get; set; }
        public GatewayEntity Gateway { get; set; }
        public int PaymentId { get; set; }
        public PaymentEntity Payment { get; set; }
        public int UserId { get; set; }
        public UserEntity User { get; set; }
    }
}
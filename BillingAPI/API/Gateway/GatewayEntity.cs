using BillingAPI.API.Order;
using BillingAPI.API.Payments;
using System.ComponentModel.DataAnnotations;

namespace BillingAPI.API.Gateway
{
    public class GatewayEntity
    {
        [Key]
        public int Id { get; set; }
        public string No { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastUpdatedAt { get; set; }
        public List<OrderEntity> Orders { get; set; }
        public List<PaymentEntity> Payments { get; set; }
    }
}
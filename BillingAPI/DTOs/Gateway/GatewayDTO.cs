using BillingAPI.DTOs.Order;
using BillingAPI.DTOs.Payment;

namespace BillingAPI.DTOs.Gateway
{
    public class GatewayDTO
    {
        public int Id { get; set; }
        public string No { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<OrderDTO> Orders { get; set; }
        public List<PaymentDTO> Payments { get; set; }
    }
}

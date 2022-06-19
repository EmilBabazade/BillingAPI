namespace BillingAPI.DTOs
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

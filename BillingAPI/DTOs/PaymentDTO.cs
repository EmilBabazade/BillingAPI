namespace BillingAPI.DTOs
{
    public class PaymentDTO
    {
        public int Id { get; set; }
        public decimal Amount { get; set; } = 0;
        public string? Description { get; set; }
        public bool IsSuccessfull { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? GatewayId { get; set; }
        public int? OrderId { get; set; }
        public int UserId { get; set; }
    }
}

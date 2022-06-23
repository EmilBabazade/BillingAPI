namespace BillingAPI.DTOs.Order
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public string No { get; set; }
        public decimal PayableAmount { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public int GatewayId { get; set; }
        public int UserId { get; set; }
    }
}

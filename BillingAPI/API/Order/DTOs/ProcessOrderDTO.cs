namespace BillingAPI.API.Order.DTOs
{
    public class ProcessOrderDTO
    {
        public string OrderNumber { get; set; }
        public int UserId { get; set; }
        public decimal PayableAmount { get; set; }
        public int GatewayId { get; set; }
        public string? Description { get; set; }
    }
}

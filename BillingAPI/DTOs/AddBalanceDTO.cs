namespace BillingAPI.DTOs
{
    public class AddBalanceDTO
    {
        public decimal Amount { get; set; }
        public int GatewayId { get; set; }
        public int UserId { get; set; }
    }
}

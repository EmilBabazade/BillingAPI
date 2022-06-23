namespace BillingAPI.DTOs.Balance
{
    public class BalanceDTO
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public int UserId { get; set; }
        public int PaymentId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

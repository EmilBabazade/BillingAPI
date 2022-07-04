using BillingAPI.API.Payments.DTOs;

namespace BillingAPI.API.Balance.DTOs
{
    public class BalanceDTO
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public int UserId { get; set; }
        public PaymentDTO Payment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

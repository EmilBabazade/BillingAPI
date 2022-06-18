namespace BillingAPI.Entities
{
    public class Balance
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastUpdatedAt { get; set; }
        public User User { get; set; }
        public Payment Payment { get; set; }
    }
}

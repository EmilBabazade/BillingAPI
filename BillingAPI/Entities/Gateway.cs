namespace BillingAPI.Entities
{
    public class Gateway
    {
        public int Id { get; set; }
        public string No { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastUpdatedAt { get; set; }
        public List<Order> Orders { get; set; }
        public List<Payment> Payments { get; set; }
    }
}
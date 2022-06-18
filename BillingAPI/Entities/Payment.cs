namespace BillingAPI.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public decimal Amount { get; set; } = 0;
        public string? Description { get; set; }
        public bool IsSuccessfull { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastUpdatedAt { get; set; }
        public Balance? Balance { get; set; }
        public Gateway? Gateway { get; set; }
        public Order? Order { get; set; }
        public User User { get; set; }
    }
}

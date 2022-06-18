namespace BillingAPI.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastUpdatedAt { get; set; }
        public List<Balance> Balances { get; set; }
        public List<Payment> Payments { get; set; }
        public List<Order> Orders { get; set; }
    }
}

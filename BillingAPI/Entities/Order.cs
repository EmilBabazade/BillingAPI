namespace BillingAPI.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string No { get; set; }
        public decimal PayableAmount { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastUpdatedAt { get; set; }
        public int GatewayId { get; set; }
        public Gateway Gateway { get; set; }
        public Payment Payment { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace BillingAPI.Entities
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }
        public decimal Amount { get; set; } = 0;
        public string? Description { get; set; }
        public bool IsSuccessfull { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastUpdatedAt { get; set; }
        public Balance? Balance { get; set; }
        public int? GatewayId { get; set; }
        public Gateway? Gateway { get; set; }
        public int? OrderId { get; set; }
        public Order? Order { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}

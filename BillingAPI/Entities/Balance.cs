using System.ComponentModel.DataAnnotations;

namespace BillingAPI.Entities
{
    public class Balance
    {
        [Key]
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastUpdatedAt { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int PaymentId { get; set; }
        public Payment Payment { get; set; }
    }
}

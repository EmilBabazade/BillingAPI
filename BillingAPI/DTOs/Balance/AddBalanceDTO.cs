using System.ComponentModel.DataAnnotations;

namespace BillingAPI.DTOs.Balance
{
    public class AddBalanceDTO
    {
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public int GatewayId { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace BillingAPI.DTOs
{
    public class AddGatewayDTO
    {
        [Required]
        public string No { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace BillingAPI.DTOs.Gateway
{
    public class AddGatewayDTO
    {
        [Required]
        public string No { get; set; }
    }
}

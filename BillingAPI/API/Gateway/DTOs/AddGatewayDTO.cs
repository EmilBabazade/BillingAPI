using System.ComponentModel.DataAnnotations;

namespace BillingAPI.API.Gateway.DTOs
{
    public class AddGatewayDTO
    {
        [Required]
        public string No { get; set; }
    }
}

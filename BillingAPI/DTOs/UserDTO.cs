namespace BillingAPI.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public BalanceDTO Balance { get; set; }
        public IEnumerable<PaymentDTO> Payments { get; set; }
        public IEnumerable<OrderDTO> Orders { get; set; }
    }
}

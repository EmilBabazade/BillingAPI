namespace BillingAPI.DTOs
{
    public class ReceiptDTO
    {
        public string OrderNo { get; set; }
        public string Date { get; set; }
        public decimal PaidAmount { get; set; }
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string UserEmail { get; set; }
    }
}

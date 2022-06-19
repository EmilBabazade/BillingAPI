using BillingAPI.Entities;

namespace BillingAPI.Data.interfaces
{
    public interface IPaymentRepository
    {
        public void Add(Payment payment);
        public Task Update(Payment payment);
        public Task<IEnumerable<Payment>> GetAll(string order = "");
        public Task<Payment> GetById(int id);
        public void DeleteById(int id);
    }
}

using BillingAPI.Entities;

namespace BillingAPI.Data.interfaces
{
    public interface IOrderRepository
    {
        public void Add(Order order);
        public Task Update(Order order);
        public Task<IEnumerable<Order>> GetAll(string order = "");
        public Task<Order> GetById(int id);
        public void DeleteById(int id);
    }
}

using BillingAPI.Entities;

namespace BillingAPI.Data.interfaces
{
    public interface IBalanceRepository
    {
        public Task Add(Balance balance);
        public Task Update(Balance balance);
        public Task<IEnumerable<Balance>> GetAll(string order = "");
        public Task<Balance> GetById(int id);
        public void DeleteById(int id);
    }
}

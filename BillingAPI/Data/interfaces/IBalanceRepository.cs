using BillingAPI.Entities;

namespace BillingAPI.Data.interfaces
{
    public interface IBalanceRepository
    {
        public void Add(Balance balance);
        public void Update(Balance balance);
        public Task<IEnumerable<Balance>> GetAll(bool asc = false, bool desc = false);
        public Task<Balance> GetById(int id);
        public void DeleteById(int id);
    }
}

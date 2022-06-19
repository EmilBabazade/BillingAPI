using BillingAPI.Data.interfaces;
using BillingAPI.Entities;
using BillingAPI.Errors;
using Microsoft.EntityFrameworkCore;

namespace BillingAPI.Data
{
    public class BalanceRepository : IBalanceRepository
    {
        private readonly DataContext _dataContext;

        public BalanceRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public void Add(Balance balance)
        {
            _dataContext.Balances.Add(balance);
        }

        public void DeleteById(int id)
        {
            Balance? balance = _dataContext.Balances.Find(id);
            if (balance == null) throw new NotFoundException("Balance not found");
            _dataContext.Balances.Remove(balance);
        }

        public async Task<IEnumerable<Balance>> GetAll(bool asc = false, bool desc = false)
        {
            if (asc)
            {
                return await _dataContext.Balances.OrderBy(b => b.Id).ToListAsync();
            }
            if (desc)
            {
                return await _dataContext.Balances.OrderByDescending(b => b.Id).ToListAsync();
            }
            return await _dataContext.Balances.ToListAsync();

        }

        public async Task<Balance> GetById(int id)
        {
            Balance? balance = await _dataContext.Balances.FindAsync(id);
            if (balance == null) throw new NotFoundException("Balance not found");
            return balance;
        }

        public void Update(Balance balance)
        {
            _dataContext.Update(balance);
        }
    }
}

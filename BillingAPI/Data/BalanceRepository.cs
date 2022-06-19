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
        public async Task Add(Balance balance)
        {
            await _dataContext.Balances.AddAsync(balance);
        }

        public void DeleteById(int id)
        {
            Balance? balance = _dataContext.Balances.Find(id);
            if (balance == null) throw new NotFoundException("Balance not found");
            _dataContext.Balances.Remove(balance);
        }

        public async Task<IEnumerable<Balance>> GetAll(string order = "")
        {
            if (order == "asc")
            {
                return await _dataContext.Balances.OrderBy(b => b.Id).ToListAsync();
            }
            if (order == "desc")
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

        public async Task<Balance> GetUserBalance(int userId)
        {
            List<Balance> balances = await _dataContext.Balances
                .Where(b => b.UserId == userId)
                .OrderByDescending(o => o.Id)
                .ToListAsync();
            if (balances.Count == 0) throw new NotFoundException("Balance not found");
            return balances[0];
        }

        public async Task Update(Balance balance)
        {
            if (!await CheckExists(balance.Id)) throw new NotFoundException("Balance not found");
            _dataContext.Update(balance);
        }

        private async Task<bool> CheckExists(int id)
        {
            return await _dataContext.Balances.AnyAsync(b => b.Id == id);
        }
    }
}

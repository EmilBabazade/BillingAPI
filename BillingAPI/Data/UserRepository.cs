using BillingAPI.Data.interfaces;
using BillingAPI.Entities;
using BillingAPI.Errors;
using Microsoft.EntityFrameworkCore;

namespace BillingAPI.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;

        public UserRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task Add(User user)
        {
            await _dataContext.Users.AddAsync(user);
        }

        public void DeleteById(int id)
        {
            User? user = _dataContext.Users.Find(id);
            if (user == null) throw new NotFoundException("User not found");
            _dataContext.Users.Remove(user);
        }

        public async Task<IEnumerable<User>> GetAll(string order = "")
        {
            if (order == "asc")
            {
                return await _dataContext.Users.OrderBy(b => b.Id).ToListAsync();
            }
            if (order == "desc")
            {
                return await _dataContext.Users.OrderByDescending(b => b.Id).ToListAsync();
            }
            return await _dataContext.Users.ToListAsync();
        }

        public async Task<User> GetById(int id)
        {
            User? user = await _dataContext.Users.FindAsync(id);
            if (user == null) throw new NotFoundException("User not found");
            return user;
        }

        public async Task Update(User user)
        {
            if (!await CheckExists(user.Id)) throw new NotFoundException("User not found");
            _dataContext.Update(user);
        }

        private async Task<bool> CheckExists(int id)
        {
            return await _dataContext.Users.AnyAsync(b => b.Id == id);
        }
    }
}

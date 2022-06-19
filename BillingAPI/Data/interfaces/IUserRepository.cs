using BillingAPI.Entities;

namespace BillingAPI.Data.interfaces
{
    public interface IUserRepository
    {
        public Task Add(User user);
        public Task Update(User user);
        public Task<IEnumerable<User>> GetAll(string order = "");
        public Task<User> GetById(int id);
        public void DeleteById(int id);
    }
}

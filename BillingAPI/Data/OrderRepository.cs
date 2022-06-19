using BillingAPI.Data.interfaces;
using BillingAPI.Entities;
using BillingAPI.Errors;
using Microsoft.EntityFrameworkCore;

namespace BillingAPI.Data
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DataContext _dataContext;

        public OrderRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public void Add(Order order)
        {
            _dataContext.Orders.Add(order);
        }

        public void DeleteById(int id)
        {
            Order? order = _dataContext.Orders.Find(id);
            if (order == null) throw new NotFoundException("Order not found");
            _dataContext.Orders.Remove(order);
        }

        public async Task<IEnumerable<Order>> GetAll(string order = "")
        {
            if (order == "asc")
            {
                return await _dataContext.Orders.OrderBy(b => b.Id).ToListAsync();
            }
            if (order == "desc")
            {
                return await _dataContext.Orders.OrderByDescending(b => b.Id).ToListAsync();
            }
            return await _dataContext.Orders.ToListAsync();
        }

        public async Task<Order> GetById(int id)
        {
            Order? order = await _dataContext.Orders.FindAsync(id);
            if (order == null) throw new NotFoundException("Order not found");
            return order;
        }

        public async Task Update(Order order)
        {
            if (!await CheckExists(order.Id)) throw new NotFoundException("Order not found");
            _dataContext.Update(order);
        }

        private async Task<bool> CheckExists(int id)
        {
            return await _dataContext.Orders.AnyAsync(b => b.Id == id);
        }
    }
}

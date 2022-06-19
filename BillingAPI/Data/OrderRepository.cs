using BillingAPI.Data.interfaces;
using BillingAPI.DTOs;
using BillingAPI.Entities;
using BillingAPI.Errors;
using Microsoft.EntityFrameworkCore;

namespace BillingAPI.Data
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DataContext _dataContext;
        private readonly IGatewayRepository _gatewayRepository;
        private readonly IUserRepository _userRepository;

        public OrderRepository(DataContext dataContext, IGatewayRepository gatewayRepository,
            IUserRepository userRepository)
        {
            _dataContext = dataContext;
            _gatewayRepository = gatewayRepository;
            _userRepository = userRepository;
        }
        public async Task Add(Order order)
        {
            await _dataContext.Orders.AddAsync(order);
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

        public async Task<ReceiptDTO> ProcessNewOrder(ProcessOrderDTO processOrderDTO)
        {
            // check order number is unique
            if (_dataContext.Orders.Any(o => o.No.Trim() == processOrderDTO.OrderNumber.Trim()))
            {
                throw new BadRequestException("An order with the given order number already exists");
            }
            // get gateway
            Gateway gateway = await _gatewayRepository.GetById(processOrderDTO.GatewayId);
            // get user
            User user = await _userRepository.GetById(processOrderDTO.UserId);
            // check user has enough balance
            throw new NotImplementedException();
        }
    }
}

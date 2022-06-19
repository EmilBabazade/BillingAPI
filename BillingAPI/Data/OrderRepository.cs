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
        private readonly IBalanceRepository _balanceRepository;
        private readonly IPaymentRepository _paymentRepository;

        public OrderRepository(DataContext dataContext, IGatewayRepository gatewayRepository,
            IUserRepository userRepository, IBalanceRepository balanceRepository,
            IPaymentRepository paymentRepository)
        {
            _dataContext = dataContext;
            _gatewayRepository = gatewayRepository;
            _userRepository = userRepository;
            _balanceRepository = balanceRepository;
            _paymentRepository = paymentRepository;
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
            Balance userBalance = await _balanceRepository.GetUserBalance(processOrderDTO.UserId);
            if (userBalance.Amount < processOrderDTO.PayableAmount)
            {
                _paymentRepository.Add(new Payment
                {
                    Amount = processOrderDTO.PayableAmount,
                    Description = processOrderDTO.Description,
                    GatewayId = processOrderDTO.GatewayId,
                    UserId = processOrderDTO.UserId,
                    IsSuccessfull = false
                });
                throw new BadRequestException("User does not have enough balance");
            }
            // create new payment and new order
            Payment payment = new Payment
            {
                Amount = processOrderDTO.PayableAmount,
                Description = processOrderDTO.Description,
                GatewayId = processOrderDTO.GatewayId,
                UserId = processOrderDTO.UserId,
                IsSuccessfull = true
            };
            _paymentRepository.Add(payment);
            await _dataContext.SaveChangesAsync();
            Order order = new Order
            {
                Description = processOrderDTO.Description,
                GatewayId = processOrderDTO.GatewayId,
                No = processOrderDTO.OrderNumber,
                PayableAmount = processOrderDTO.PayableAmount,
                PaymentId = payment.Id,
                UserId = processOrderDTO.UserId
            };
            Add(order);
            // subtract from user balance
            Balance? balance = new Balance
            {
                Amount = userBalance.Amount - processOrderDTO.PayableAmount,
                PaymentId = payment.Id,
                UserId = user.Id
            };
            _balanceRepository.Add(balance);
            // return the receipt
            return new ReceiptDTO
            {
                Date = order.CreatedAt.ToString(),
                OrderNo = order.No,
                PaidAmount = order.PayableAmount,
                UserEmail = user.Email,
                UserName = user.Name,
                UserSurname = user.Surname
            };
        }
    }
}

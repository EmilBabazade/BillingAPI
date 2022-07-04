using BillingAPI.API.Balance;
using BillingAPI.API.Gateway;
using BillingAPI.API.Order.DTOs;
using BillingAPI.API.Payments;
using BillingAPI.API.User;
using BillingAPI.Data;
using BillingAPI.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BillingAPI.API.Order.Handlers
{
    public class AddOrderHandler : IRequestHandler<AddOrderCommand, ReceiptDTO>
    {
        private readonly DataContext _dataContext;

        public AddOrderHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<ReceiptDTO> Handle(AddOrderCommand request, CancellationToken cancellationToken)
        {
            ProcessOrderDTO? processOrderDTO = request.ProcessOrderDTO;
            await CheckOrderNumberIsUnique(processOrderDTO);
            await GetGateway(processOrderDTO);
            UserEntity? user = await GetUser(processOrderDTO);
            BalanceEntity userBalance = await CheckUserHasEnoughBalance(processOrderDTO, user);
            PaymentEntity payment = await AddNewPayment(processOrderDTO);
            OrderEntity order = await AddOrder(processOrderDTO, user, userBalance, payment);
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

        private async Task<OrderEntity> AddOrder(ProcessOrderDTO processOrderDTO, UserEntity user, BalanceEntity userBalance, PaymentEntity payment)
        {
            OrderEntity order = new OrderEntity
            {
                Description = processOrderDTO.Description,
                GatewayId = processOrderDTO.GatewayId,
                No = processOrderDTO.OrderNumber,
                PayableAmount = processOrderDTO.PayableAmount,
                PaymentId = payment.Id,
                UserId = processOrderDTO.UserId
            };
            _dataContext.Add(order);
            SubtractFromUserBalance(processOrderDTO, user, userBalance, payment);
            await _dataContext.SaveChangesAsync();
            return order;
        }

        private void SubtractFromUserBalance(ProcessOrderDTO processOrderDTO, UserEntity user, BalanceEntity userBalance, PaymentEntity payment)
        {
            BalanceEntity? balance = new BalanceEntity
            {
                Amount = userBalance.Amount - processOrderDTO.PayableAmount,
                PaymentId = payment.Id,
                UserId = user.Id
            };
            _dataContext.Add(balance);
        }

        private async Task<PaymentEntity> AddNewPayment(ProcessOrderDTO processOrderDTO)
        {
            PaymentEntity payment = new PaymentEntity
            {
                Amount = processOrderDTO.PayableAmount,
                Description = processOrderDTO.Description,
                GatewayId = processOrderDTO.GatewayId,
                UserId = processOrderDTO.UserId,
                IsSuccessfull = true
            };
            _dataContext.Add(payment);
            await _dataContext.SaveChangesAsync();
            return payment;
        }

        private async Task<BalanceEntity> CheckUserHasEnoughBalance(ProcessOrderDTO processOrderDTO, UserEntity user)
        {
            List<BalanceEntity>? balances = await _dataContext.Balances.OrderByDescending(b => b.Id).Where(b => b.UserId == user.Id).ToListAsync();
            if (balances.Count == 0) throw new NotFoundException("This user does not have balance");
            BalanceEntity? userBalance = balances[0];
            if (userBalance.Amount < processOrderDTO.PayableAmount)
            {
                _dataContext.Add(new PaymentEntity
                {
                    Amount = processOrderDTO.PayableAmount,
                    Description = processOrderDTO.Description,
                    GatewayId = processOrderDTO.GatewayId,
                    UserId = processOrderDTO.UserId,
                    IsSuccessfull = false
                });
                await _dataContext.SaveChangesAsync();
                throw new BadRequestException("User does not have enough balance");
            }

            return userBalance;
        }

        private async Task<UserEntity> GetUser(ProcessOrderDTO processOrderDTO)
        {
            UserEntity? user = await _dataContext.Users.FindAsync(processOrderDTO.UserId);
            if (user == null) throw new NotFoundException("User not found");
            return user;
        }

        private async Task GetGateway(ProcessOrderDTO processOrderDTO)
        {
            GatewayEntity? gateway = await _dataContext.Gateways.FindAsync(processOrderDTO.GatewayId);
            if (gateway == null) throw new NotFoundException("Gateway not found");
        }

        private async Task CheckOrderNumberIsUnique(ProcessOrderDTO processOrderDTO)
        {
            if (await _dataContext.Orders.AnyAsync(o => o.No.Trim() == processOrderDTO.OrderNumber.Trim()))
                throw new BadRequestException("An order with the given order number already exists");
        }
    }
}

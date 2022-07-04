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
            // check order number is unique
            if (_dataContext.Orders.Any(o => o.No.Trim() == processOrderDTO.OrderNumber.Trim()))
                throw new BadRequestException("An order with the given order number already exists");
            // get gateway
            GatewayEntity? gateway = await _dataContext.Gateways.FindAsync(processOrderDTO.GatewayId);
            if (gateway == null) throw new NotFoundException("Gateway not found");
            // get user
            UserEntity? user = await _dataContext.Users.FindAsync(processOrderDTO.UserId);
            if (user == null) throw new NotFoundException("User not found");
            // check user has enough balance
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
            // create new payment and new order
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
            // subtract from user balance
            BalanceEntity? balance = new BalanceEntity
            {
                Amount = userBalance.Amount - processOrderDTO.PayableAmount,
                PaymentId = payment.Id,
                UserId = user.Id
            };
            _dataContext.Add(balance);
            await _dataContext.SaveChangesAsync();
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

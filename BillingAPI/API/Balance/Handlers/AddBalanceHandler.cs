using AutoMapper;
using BillingAPI.API.Balance.DTOs;
using BillingAPI.API.Payments;
using BillingAPI.Data;
using BillingAPI.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BillingAPI.API.Balance.Handlers
{
    public class AddBalanceHandler : IRequestHandler<AddBalanceCommand, BalanceDTO>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public AddBalanceHandler(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }
        public async Task<BalanceDTO> Handle(AddBalanceCommand request, CancellationToken cancellationToken)
        {
            await CheckUserExists(request);
            await CheckGatewayExists(request);
            PaymentEntity payment = await AddPayment(request);
            BalanceEntity balance = await AddPayment(request, payment);
            return _mapper.Map<BalanceDTO>(balance);
        }

        private async Task<BalanceEntity> AddPayment(AddBalanceCommand request, PaymentEntity payment)
        {
            List<BalanceEntity>? oldBalance = await _dataContext.Balances
                            .Where(b => b.UserId == request.AddBalanceDTO.UserId)
                            .OrderByDescending(b => b.Id)
                            .ToListAsync();
            BalanceEntity balance = new()
            {
                Amount = oldBalance.Count > 0 ? payment.Amount + oldBalance[0].Amount : payment.Amount,
                PaymentId = payment.Id,
                UserId = payment.UserId
            };
            _dataContext.Add(balance);
            await _dataContext.SaveChangesAsync();
            return balance;
        }

        private async Task<PaymentEntity> AddPayment(AddBalanceCommand request)
        {
            PaymentEntity payment = new()
            {
                Amount = request.AddBalanceDTO.Amount,
                UserId = request.AddBalanceDTO.UserId,
                GatewayId = request.AddBalanceDTO.GatewayId,
                IsSuccessfull = true
            };
            _dataContext.Add(payment);
            await _dataContext.SaveChangesAsync();
            return payment;
        }

        private async Task CheckGatewayExists(AddBalanceCommand request)
        {
            if (!await _dataContext.Gateways.AnyAsync(g => g.Id == request.AddBalanceDTO.GatewayId))
                throw new NotFoundException("Gateway not found!");
        }

        private async Task CheckUserExists(AddBalanceCommand request)
        {
            if (!await _dataContext.Users.AnyAsync(u => u.Id == request.AddBalanceDTO.UserId))
                throw new NotFoundException("User not found!");
        }
    }
}

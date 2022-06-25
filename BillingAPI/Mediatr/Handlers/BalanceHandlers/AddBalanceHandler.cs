using AutoMapper;
using BillingAPI.Data;
using BillingAPI.DTOs.Balance;
using BillingAPI.Entities;
using BillingAPI.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BillingAPI.Mediatr.Handlers.BalanceHandlers
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
            // check if user and gateway exists 
            if (!await _dataContext.Users.AnyAsync(u => u.Id == request.AddBalanceDTO.UserId))
                throw new NotFoundException("User not found!");
            if (!await _dataContext.Gateways.AnyAsync(g => g.Id == request.AddBalanceDTO.GatewayId))
                throw new NotFoundException("Gateway not found!");
            // add payment
            Payment payment = new()
            {
                Amount = request.AddBalanceDTO.Amount,
                UserId = request.AddBalanceDTO.UserId,
                GatewayId = request.AddBalanceDTO.GatewayId,
                IsSuccessfull = true
            };
            _dataContext.Add(payment);
            await _dataContext.SaveChangesAsync();
            // add balance 
            List<Balance>? oldBalance = await _dataContext.Balances
                .Where(b => b.UserId == request.AddBalanceDTO.UserId)
                .OrderByDescending(b => b.Id)
                .ToListAsync();
            Balance balance = new()
            {
                Amount = oldBalance.Count > 0 ? payment.Amount + oldBalance[0].Amount : payment.Amount,
                PaymentId = payment.Id,
                UserId = payment.UserId
            };
            _dataContext.Add(balance);
            await _dataContext.SaveChangesAsync();
            return _mapper.Map<BalanceDTO>(balance);
        }
    }
}

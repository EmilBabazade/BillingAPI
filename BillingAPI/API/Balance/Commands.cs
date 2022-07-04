using BillingAPI.API.Balance.DTOs;
using MediatR;

namespace BillingAPI.API.Balance
{
    public record AddBalanceCommand(AddBalanceDTO AddBalanceDTO) : IRequest<BalanceDTO>;
}

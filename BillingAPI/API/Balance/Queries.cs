using BillingAPI.API.Balance.DTOs;
using MediatR;

namespace BillingAPI.API.Balance
{
    public record GetBalancesQuery(int? userId = null, string order = "") : IRequest<IEnumerable<BalanceDTO>>;
    public record GetBalanceQuery(int id) : IRequest<BalanceDTO>;
}

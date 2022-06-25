using BillingAPI.DTOs.Balance;
using BillingAPI.DTOs.Gateway;
using BillingAPI.DTOs.User;
using MediatR;

namespace BillingAPI.Mediatr
{
    public record GetGatewaysQuery(string Order = "") : IRequest<IEnumerable<GatewayDTO>>;
    public record GetGatewayByIdQuery(int Id) : IRequest<GatewayDTO>;
    public record GetUsersQuery(string Order = "") : IRequest<IEnumerable<UserDTO>>;
    public record GetUserByIdQuery(int Id) : IRequest<UserDTO>;
    public record GetBalancesQuery(int? userId = null, string order = "") : IRequest<IEnumerable<BalanceDTO>>;
    public record GetBalanceQuery(int id) : IRequest<BalanceDTO>;
}

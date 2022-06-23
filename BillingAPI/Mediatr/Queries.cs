using BillingAPI.DTOs.Gateway;
using BillingAPI.DTOs.User;
using MediatR;

namespace BillingAPI.Mediatr
{
    public record GetGatewaysQuery(string Order = "") : IRequest<IEnumerable<GatewayDTO>>;
    public record GetGatewayByIdQuery(int Id) : IRequest<GatewayDTO>;
    public record GetUsersQuery(string Order = "") : IRequest<IEnumerable<UserDTO>>;
    public record GetUserByİdQuery(int Id) : IRequest<UserDTO>;
}

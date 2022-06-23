using BillingAPI.DTOs;
using MediatR;

namespace BillingAPI.Mediatr
{
    public record GetGatewaysQuery(string order = "") : IRequest<IEnumerable<GatewayDTO>>;
    public record GetGatewayByIdQuery(int id) : IRequest<GatewayDTO>;
    public record GetUsersQuery(string order = "") : IRequest<IEnumerable<UserDTO>>;
}

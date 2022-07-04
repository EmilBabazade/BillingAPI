using BillingAPI.API.Gateway.DTOs;
using MediatR;

namespace BillingAPI.API.Gateway
{
    public record GetGatewaysQuery(string Order = "") : IRequest<IEnumerable<GatewayDTO>>;
    public record GetGatewayByIdQuery(int Id) : IRequest<GatewayDTO>;
}

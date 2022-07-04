using BillingAPI.API.Gateway.DTOs;
using MediatR;

namespace BillingAPI.API.Gateway
{
    public record AddGatewayCommand(AddGatewayDTO AddGatewayDTO) : IRequest<GatewayDTO>;
    public record DeleteGatewayCommand(int id) : IRequest<Unit>;
    public record UpdateGatewayCommand(UpdateGatewayDTO UpdateGatewayDTO) : IRequest<GatewayDTO>;
}

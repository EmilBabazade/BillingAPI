using BillingAPI.DTOs;
using MediatR;

namespace BillingAPI.Mediatr
{
    public record AddGatewayCommand(AddGatewayDTO AddGatewayDTO) : IRequest<GatewayDTO>;
    public record DeleteGatewayCommand(int id) : IRequest<Unit>;
}

using BillingAPI.DTOs.Gateway;
using BillingAPI.DTOs.User;
using MediatR;

namespace BillingAPI.Mediatr
{
    public record AddGatewayCommand(AddGatewayDTO AddGatewayDTO) : IRequest<GatewayDTO>;
    public record DeleteGatewayCommand(int id) : IRequest<Unit>;
    public record UpdateGatewayCommand(UpdateGatewayDTO UpdateGatewayDTO) : IRequest<GatewayDTO>;
    public record AddUserCommand(AddUserDTO AddUserDTO) : IRequest<UserDTO>;
    public record UpdateUserCommand(UpdateUserDTO UpdateUserDTO) : IRequest<UserDTO>;
    public record DeleteUserCommand(int Id) : IRequest<Unit>;
}

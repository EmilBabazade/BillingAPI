using BillingAPI.DTOs.Account;
using BillingAPI.DTOs.Balance;
using BillingAPI.DTOs.Gateway;
using BillingAPI.DTOs.Order;
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
    public record AddBalanceCommand(AddBalanceDTO AddBalanceDTO) : IRequest<BalanceDTO>;
    public record AddOrderCommand(ProcessOrderDTO ProcessOrderDTO) : IRequest<ReceiptDTO>;
    public record RegisterCommand(RegisterDTO RegisterDTO) : IRequest<Unit>;
    public record LoginCommand(LoginDTO LoginDTO) : IRequest<AccountDTO>;
}

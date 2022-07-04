using BillingAPI.API.User.DTOs;
using MediatR;

namespace BillingAPI.API.User
{
    public record AddUserCommand(AddUserDTO AddUserDTO) : IRequest<UserDTO>;
    public record UpdateUserCommand(UpdateUserDTO UpdateUserDTO) : IRequest<UserDTO>;
    public record DeleteUserCommand(int Id) : IRequest<Unit>;
}

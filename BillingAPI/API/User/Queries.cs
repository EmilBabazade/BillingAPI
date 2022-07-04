using BillingAPI.API.User.DTOs;
using MediatR;

namespace BillingAPI.API.User
{
    public record GetUsersQuery(string Order = "") : IRequest<IEnumerable<UserDTO>>;
    public record GetUserByIdQuery(int Id) : IRequest<UserDTO>;
}

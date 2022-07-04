using BillingAPI.API.Account.DTOs;
using MediatR;

namespace BillingAPI.API.Account
{
    public record RegisterCommand(RegisterDTO RegisterDTO) : IRequest<Unit>;
    public record LoginCommand(LoginDTO LoginDTO) : IRequest<AccountDTO>;
}

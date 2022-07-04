using BillingAPI.API.Account.DTOs;
using BillingAPI.Data;
using BillingAPI.Errors;
using BillingAPI.Services.jwt;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BillingAPI.API.Account.Handlers
{
    public class LoginHandler : IRequestHandler<LoginCommand, AccountDTO>
    {
        private readonly DataContext _dataContext;
        private readonly IJWTService _jwt;

        public LoginHandler(DataContext dataContext, IJWTService jwt)
        {
            _dataContext = dataContext;
            _jwt = jwt;
        }
        public async Task<AccountDTO> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            LoginDTO loginDTO = request.LoginDTO;
            // check user exists
            AccountEntity? user = await _dataContext.Accounts.SingleOrDefaultAsync(a => a.Username.Trim() == request.LoginDTO.Username);
            if (user == null)
                throw new BadRequestException("User not found");

            // check user password
            if (!_jwt.VerifyPasswordHash(loginDTO.Password, user.PasswordHash, user.PasswordSalt))
                throw new BadRequestException("Wrong password");

            return new AccountDTO
            {
                Token = _jwt.CreateToken(user),
                UserName = loginDTO.Username,
            };
        }
    }
}

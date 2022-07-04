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

            AccountEntity? user = await GetUser(request);
            CheckPassword(loginDTO, user);

            return new AccountDTO
            {
                Token = _jwt.CreateToken(user),
                UserName = loginDTO.Username,
            };
        }

        private void CheckPassword(LoginDTO loginDTO, AccountEntity user)
        {
            if (!_jwt.VerifyPasswordHash(loginDTO.Password, user.PasswordHash, user.PasswordSalt))
                throw new BadRequestException("Wrong password");
        }

        private async Task<AccountEntity> GetUser(LoginCommand request)
        {
            AccountEntity? user = await _dataContext.Accounts.SingleOrDefaultAsync(a => a.Username.Trim() == request.LoginDTO.Username);
            if (user == null)
                throw new BadRequestException("User not found");
            return user;
        }
    }
}

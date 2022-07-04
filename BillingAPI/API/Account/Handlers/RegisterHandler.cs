using BillingAPI.Data;
using BillingAPI.Errors;
using BillingAPI.Services.jwt;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BillingAPI.API.Account.Handlers
{
    public class RegisterHandler : IRequestHandler<RegisterCommand, Unit>
    {
        private readonly DataContext _dataContext;
        private readonly IJWTService _jwt;

        public RegisterHandler(DataContext dataContext, IJWTService jwt)
        {
            _dataContext = dataContext;
            _jwt = jwt;
        }
        public async Task<Unit> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            // check if username is unique
            if (await _dataContext.Accounts.AnyAsync(a => a.Username.Trim() == request.RegisterDTO.Username.Trim()))
                throw new BadRequestException("Account with given username already exists");
            _jwt.CreatePasswordHash(request.RegisterDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);
            _dataContext.Add(new AccountEntity
            {
                Username = request.RegisterDTO.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            });
            await _dataContext.SaveChangesAsync();
            return Unit.Value;
        }
    }
}

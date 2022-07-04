using BillingAPI.API.Account;

namespace BillingAPI.Services.jwt
{
    public interface IJWTService
    {
        public string CreateToken(AccountEntity account);
        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
    }
}

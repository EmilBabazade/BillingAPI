using BillingAPI.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BillingAPI.Services.jwt
{
    public class JWTService : IJWTService
    {
        private readonly IConfiguration _configuration;

        public JWTService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using HMACSHA512? hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        public string CreateToken(Account account)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, account.Username)
            };

            SymmetricSecurityKey? key = new(Encoding.UTF8.GetBytes(_configuration.GetSection("jwt").Value));
            SigningCredentials? creds = new(key, SecurityAlgorithms.HmacSha512Signature);
            JwtSecurityToken? token = new(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using HMACSHA512? hmac = new HMACSHA512(passwordSalt);
            byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }
}

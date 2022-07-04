using BillingAPI.API.Balance;
using BillingAPI.API.Gateway;
using BillingAPI.API.Payments;
using BillingAPI.API.User;
using Microsoft.EntityFrameworkCore;

namespace BillingAPI.Data
{
    public class Seed
    {
        public static async Task SeedData(DataContext dataContext)
        {
            if (await dataContext.Users.AnyAsync()) return;
            List<GatewayEntity>? gateways = new List<GatewayEntity>()
            {
                new GatewayEntity
                {
                    No = "gt1",
                },
                new GatewayEntity
                {
                    No = "gt2"
                },
                new GatewayEntity
                {
                    No = "gt3"
                }
            };
            dataContext.AddRange(gateways);
            dataContext.SaveChanges();
            List<UserEntity>? users = new List<UserEntity>()
            {
                new UserEntity
                {
                    Email = "emil.babazade@gmail.com",
                    Name = "Emil",
                    Surname = "Babazade"
                },
                new UserEntity
                {
                    Email = "admin@admin.com",
                    Name = "admin",
                    Surname = "admin"
                }
            };
            dataContext.AddRange(users);
            dataContext.SaveChanges();
            List<PaymentEntity>? payments = new List<PaymentEntity>()
            {
                new PaymentEntity
                {
                    Amount = 100,
                    Description = "adding to balance",
                    GatewayId = gateways[0].Id,
                    IsSuccessfull = true,
                    UserId = users[0].Id
                },
                new PaymentEntity
                {
                    Amount = 200,
                    Description = "adding to balance",
                    GatewayId = gateways[1].Id,
                    IsSuccessfull = true,
                    UserId = users[1].Id
                }
            };
            dataContext.AddRange(payments);
            dataContext.SaveChanges();
            List<BalanceEntity>? balances = new List<BalanceEntity>()
            {
                new BalanceEntity
                {
                    Amount = payments[0].Amount,
                    PaymentId = payments[0].Id,
                    UserId = users[0].Id
                },
                new BalanceEntity
                {
                    Amount = payments[1].Amount,
                    PaymentId = payments[1].Id,
                    UserId = users[1].Id
                }
            };
            dataContext.AddRange(balances);
            await dataContext.SaveChangesAsync();
        }
    }
}

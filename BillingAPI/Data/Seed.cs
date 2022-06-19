using BillingAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace BillingAPI.Data
{
    public class Seed
    {
        public static async Task SeedData(DataContext dataContext)
        {
            if (await dataContext.Users.AnyAsync()) return;
            List<Gateway>? gateways = new List<Gateway>()
            {
                new Gateway
                {
                    No = "gt1",
                },
                new Gateway
                {
                    No = "gt2"
                },
                new Gateway
                {
                    No = "gt3"
                }
            };
            dataContext.AddRange(gateways);
            dataContext.SaveChanges();
            List<User>? users = new List<User>()
            {
                new User
                {
                    Email = "emil.babazade@gmail.com",
                    Name = "Emil",
                    Surname = "Babazade"
                },
                new User
                {
                    Email = "admin@admin.com",
                    Name = "admin",
                    Surname = "admin"
                }
            };
            dataContext.AddRange(users);
            dataContext.SaveChanges();
            List<Payment>? payments = new List<Payment>()
            {
                new Payment
                {
                    Amount = 100,
                    Description = "adding to balance",
                    GatewayId = gateways[0].Id,
                    IsSuccessfull = true,
                    UserId = users[0].Id
                },
                new Payment
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
            List<Balance>? balances = new List<Balance>()
            {
                new Balance
                {
                    Amount = payments[0].Amount,
                    PaymentId = payments[0].Id,
                    UserId = users[0].Id
                },
                new Balance
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

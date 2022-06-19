using BillingAPI.Data;
using BillingAPI.Entities;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BillingAPITest.Tests.Repository
{
    public class BaseRepositoryTests
    {
        protected DataContext _dataContext;

        [OneTimeSetUp]
        public void SetupDb()
        {
            DbContextOptions<DataContext>? options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

            _dataContext = new DataContext(options);
            _dataContext.Database.EnsureCreated();
        }

        [TearDown]
        public async Task ClearData()
        {
            _dataContext.Balances.RemoveRange();
            _dataContext.Gateways.RemoveRange();
            _dataContext.Orders.RemoveRange();
            _dataContext.Payments.RemoveRange();
            _dataContext.Users.RemoveRange();
            await _dataContext.SaveChangesAsync();
        }

        [SetUp]
        public async Task SeedData()
        {
            Gateway? gt1 = new Gateway
            {
                No = "gt1"
            };
            Gateway? gt2 = new Gateway
            {
                No = "gt2"
            };
            Gateway? gt3 = new Gateway
            {
                No = "gt3"
            };
            // add gateways
            _dataContext.AddRange(new List<Gateway> { gt1, gt2, gt3 });
            // add users
            User? u1 = new User
            {
                Email = "bob@bob.com",
                Name = "bob",
                Surname = "bobby"
            };
            User? u2 = new User
            {
                Email = "bob2@bob.com",
                Name = "bob2",
                Surname = "bobby2"
            };
            User? u3 = new User
            {
                Email = "bob3@bob.com",
                Name = "bob3",
                Surname = "bobby3"
            };
            _dataContext.AddRange(new List<User> { u1, u2, u3 });
            // add payments for balances
            Payment? p1 = new Payment
            {
                Amount = 100,
                Description = "adding to balance",
                GatewayId = gt1.Id,
                IsSuccessfull = true,
                UserId = u1.Id
            };
            Payment? p2 = new Payment
            {
                Amount = 150,
                Description = "adding to balance",
                GatewayId = gt2.Id,
                IsSuccessfull = true,
                UserId = u2.Id
            };
            Payment? p3 = new Payment
            {
                Amount = 50,
                Description = "adding to balance",
                GatewayId = gt3.Id,
                IsSuccessfull = true,
                UserId = u3.Id
            };
            _dataContext.AddRange(new List<Payment> { p1, p2, p3 });
            // add balance
            Balance? b1 = new Balance
            {
                Amount = p1.Amount,
                PaymentId = p1.Id,
                UserId = u1.Id
            };
            Balance? b2 = new Balance
            {
                Amount = p2.Amount,
                PaymentId = p2.Id,
                UserId = u2.Id
            };
            Balance? b3 = new Balance
            {
                Amount = p2.Amount,
                PaymentId = p2.Id,
                UserId = u2.Id
            };
            _dataContext.AddRange(new List<Balance> { b1, b2, b3 });
            await _dataContext.SaveChangesAsync();
        }

        [OneTimeTearDown]
        public void DeleteDb()
        {
            _dataContext.Database.EnsureDeleted();
        }
    }
}

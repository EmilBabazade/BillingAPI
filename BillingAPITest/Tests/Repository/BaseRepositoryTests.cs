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
            // below code does not work with in memory database, it only works with real databases
            //_dataContext.Database.ExecuteSqlRaw("DELETE FROM BALANCES");
            //_dataContext.Database.ExecuteSqlRaw("DELETE FROM GATEWAYS");
            //_dataContext.Database.ExecuteSqlRaw("DELETE FROM ORDERS");
            //_dataContext.Database.ExecuteSqlRaw("DELETE FROM PAYMENTS");
            //_dataContext.Database.ExecuteSqlRaw("DELETE FROM USERS");

            _dataContext.Balances.RemoveRange(await _dataContext.Balances.ToListAsync());
            _dataContext.Gateways.RemoveRange(await _dataContext.Gateways.ToListAsync());
            _dataContext.Orders.RemoveRange(await _dataContext.Orders.ToListAsync());
            _dataContext.Payments.RemoveRange(await _dataContext.Payments.ToListAsync());
            _dataContext.Users.RemoveRange(await _dataContext.Users.ToListAsync());
            await _dataContext.SaveChangesAsync();
        }

        protected List<Gateway> _gateways = new();
        protected List<Balance> _balances = new();
        protected List<User> _users = new();
        protected List<Payment> _payments = new();
        protected List<Order> _orders = new();

        [SetUp]
        public async Task SeedData()
        {
            // add gateways
            _gateways = new List<Gateway>()
            {
                new Gateway
                {
                    No = "gt1"
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
            _dataContext.AddRange(_gateways);
            // add users
            _users = new()
            {
                new User
                {
                    Email = "bob@bob.com",
                    Name = "bob",
                    Surname = "bobby"
                },
                new User
                {
                    Email = "bob2@bob.com",
                    Name = "bob2",
                    Surname = "bobby2"
                },
                new User
                {
                    Email = "bob3@bob.com",
                    Name = "bob3",
                    Surname = "bobby3"
                },
            };
            _dataContext.AddRange(_users);
            // add payments for balances
            _payments = new()
            {
                new Payment
                {
                    Amount = 100,
                    Description = "adding to balance",
                    GatewayId = _gateways[0].Id,
                    IsSuccessfull = true,
                    UserId = _users[0].Id
                },
                new Payment
                {
                    Amount = 150,
                    Description = "adding to balance",
                    GatewayId = _gateways[1].Id,
                    IsSuccessfull = true,
                    UserId = _users[1].Id
                },
                new Payment
                {
                    Amount = 50,
                    Description = "adding to balance",
                    GatewayId = _gateways[2].Id,
                    IsSuccessfull = true,
                    UserId = _users[2].Id
                }
            };

            _dataContext.AddRange(_payments);
            // add balance
            _balances = new()
            {
                new Balance
                {
                    Amount = _payments[0].Amount,
                    PaymentId = _payments[0].Id,
                    UserId = _users[0].Id
                },
                new Balance
                {
                    Amount = _payments[1].Amount,
                    PaymentId = _payments[1].Id,
                    UserId = _users[1].Id
                },
                new Balance
                {
                    Amount = _payments[2].Amount,
                    PaymentId = _payments[2].Id,
                    UserId = _users[2].Id
                },
            };
            _dataContext.AddRange(_balances);
            // add payments
            List<Payment>? orderPayments = new List<Payment>
            {
                new Payment
                {
                    Amount = -20,
                    IsSuccessfull = true,
                    UserId = _users[0].Id
                },
                new Payment
                {
                    Amount = -30,
                    IsSuccessfull = true,
                    UserId = _users[1].Id
                },
                new Payment
                {
                    Amount = -15,
                    IsSuccessfull = true,
                    UserId = _users[0].Id
                },
            };
            await _dataContext.AddRangeAsync(orderPayments);
            _payments.AddRange(orderPayments);
            _orders = new List<Order>()
            {
                new Order
                {
                    No = "234",
                    PayableAmount = -20,
                    UserId = _users[0].Id
                },
                new Order
                {
                    No = "235",
                    PayableAmount = -30,
                    UserId = _users[1].Id
                },
                new Order
                {
                    No = "236",
                    PayableAmount = -15,
                    UserId = _users[2].Id
                },
            };
            await _dataContext.AddRangeAsync(_orders);
            // add orders
            await _dataContext.SaveChangesAsync();
        }

        [OneTimeTearDown]
        public void DeleteDb()
        {
            _dataContext.Database.EnsureDeleted();
        }
    }
}

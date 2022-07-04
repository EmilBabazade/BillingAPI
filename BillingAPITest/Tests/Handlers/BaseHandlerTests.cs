using AutoMapper;
using BillingAPI.API.Balance;
using BillingAPI.API.Gateway;
using BillingAPI.API.Order;
using BillingAPI.API.Payments;
using BillingAPI.API.User;
using BillingAPI.Data;
using BillingAPI.Helpers;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BillingAPITest.Tests
{
    public class BaseHandlerTests
    {
        protected DataContext _dataContext;
        protected IMapper _mapper;

        [OneTimeSetUp]
        public void SetupDb()
        {
            DbContextOptions<DataContext>? options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dataContext = new DataContext(options);
            _dataContext.Database.EnsureCreated();

            MapperConfiguration? mapFactory = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfiles());
            });
            _mapper = mapFactory.CreateMapper();
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

        protected List<GatewayEntity> _gateways = new();
        protected List<BalanceEntity> _balances = new();
        protected List<UserEntity> _users = new();
        protected List<PaymentEntity> _payments = new();
        protected List<OrderEntity> _orders = new();

        [SetUp]
        public async Task SeedData()
        {
            // add gateways
            _gateways = new List<GatewayEntity>()
            {
                new GatewayEntity
                {
                    No = "gt1"
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
            _dataContext.AddRange(_gateways);
            // add users
            _users = new()
            {
                new UserEntity
                {
                    Email = "bob@bob.com",
                    Name = "bob",
                    Surname = "bobby"
                },
                new UserEntity
                {
                    Email = "bob2@bob.com",
                    Name = "bob2",
                    Surname = "bobby2"
                },
                new UserEntity
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
                new PaymentEntity
                {
                    Amount = 100,
                    Description = "adding to balance",
                    GatewayId = _gateways[0].Id,
                    IsSuccessfull = true,
                    UserId = _users[0].Id
                },
                new PaymentEntity
                {
                    Amount = 150,
                    Description = "adding to balance",
                    GatewayId = _gateways[1].Id,
                    IsSuccessfull = true,
                    UserId = _users[1].Id
                },
                new PaymentEntity
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
                new BalanceEntity
                {
                    Amount = _payments[0].Amount,
                    PaymentId = _payments[0].Id,
                    UserId = _users[0].Id
                },
                new BalanceEntity
                {
                    Amount = _payments[1].Amount,
                    PaymentId = _payments[1].Id,
                    UserId = _users[1].Id
                },
                new BalanceEntity
                {
                    Amount = _payments[2].Amount,
                    PaymentId = _payments[2].Id,
                    UserId = _users[2].Id
                },
            };
            _dataContext.AddRange(_balances);
            // add payments
            List<PaymentEntity>? orderPayments = new List<PaymentEntity>
            {
                new PaymentEntity
                {
                    Amount = -20,
                    IsSuccessfull = true,
                    UserId = _users[0].Id
                },
                new PaymentEntity
                {
                    Amount = -30,
                    IsSuccessfull = true,
                    UserId = _users[1].Id
                },
                new PaymentEntity
                {
                    Amount = -15,
                    IsSuccessfull = true,
                    UserId = _users[0].Id
                },
            };
            await _dataContext.AddRangeAsync(orderPayments);
            _payments.AddRange(orderPayments);
            _orders = new List<OrderEntity>()
            {
                new OrderEntity
                {
                    No = "234",
                    PayableAmount = -20,
                    UserId = _users[0].Id
                },
                new OrderEntity
                {
                    No = "235",
                    PayableAmount = -30,
                    UserId = _users[1].Id
                },
                new OrderEntity
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

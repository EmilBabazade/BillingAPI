using BillingAPI.Data;
using BillingAPI.DTOs;
using BillingAPI.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BillingAPITest.Tests.Repository
{
    public class OrderRepositoryTests : BaseRepositoryTests
    {
        private OrderRepository _orderRepository;

        [SetUp]
        public void SetupRepository()
        {
            _orderRepository = new OrderRepository(
                _dataContext, new GatewayRepository(_dataContext), new UserRepository(_dataContext),
                new BalanceRepository(_dataContext), new PaymentRepository(_dataContext)
                );
        }

        //// add should add
        [Test]
        public async Task AddingNewOrderShouldAddNewOrderToOrdersTable()
        {
            // Arrange
            Order? g = new Order
            {
                No = "od1"
            };

            // Act
            _orderRepository.Add(g);
            await _dataContext.SaveChangesAsync();
            Order? newOrder = await _dataContext.Orders.FindAsync(g.Id);
            System.Collections.Generic.List<Order>? ordersInDb = await _dataContext.Orders.ToListAsync();

            // Assert
            newOrder.Should().NotBeNull();
            ordersInDb.Count.Should().Be(_orders.Count + 1);
            newOrder.Should().Be(g);
        }

        //// delete by id should delete
        [Test]
        public async Task DeleteByIdShouldLessenLengthBy1AndDelete()
        {
            // Arrange
            int deleteId = _orders[0].Id;

            // Act
            _orderRepository.DeleteById(deleteId);
            await _dataContext.SaveChangesAsync();
            Order? deletedOrder = await _dataContext.Orders.FindAsync(deleteId);
            List<Order>? ordersInDB = await _dataContext.Orders.ToListAsync();

            // Assert
            deletedOrder.Should().BeNull();
            ordersInDB.Count.Should().Be(_orders.Count - 1);
        }

        // delete by id should throw exception
        [Test]
        public void DeleteByIdShouldThrowNotFoundExceptionForNonExistentOrder()
        {
            // Arrange
            int deleteId = -99999;
            string? type = "";

            // Act
            try
            {
                _orderRepository.DeleteById(deleteId);
            }
            catch (Exception ex)
            {
                type = ex.GetType()?.FullName;
            }

            // Assert
            type.Should().Be("BillingAPI.Errors.NotFoundException");
        }

        // get all should get all
        [Test]
        public async Task GetAllShouldReturnSameLengthAsInitialList()
        {
            // Arrange
            int oldCount = _orders.Count;

            // Act
            IEnumerable<Order>? ordersInDB = await _orderRepository.GetAll();

            // Assert
            ordersInDB.Select(b => b).ToList().Count.Should().Be(oldCount);
        }

        // get all asc should return ascending
        [Test]
        [TestCase("asc")]
        [TestCase("desc")]
        public async Task GetAllAscendingShouldReturnInAscendingOrderAndDescendingShouldReturnInDescendingOrder(string order)
        {
            // Arrange

            // Act
            IEnumerable<Order>? ordersInDB = await _orderRepository.GetAll(order);
            List<int>? ids = ordersInDB.Select(b => b.Id).ToList();


            // Assert
            if (order == "asc")
            {
                ids[2].Should().BeGreaterThan(ids[1]);
                ids[1].Should().BeGreaterThan(ids[0]);
            }
            else
            {
                ids[2].Should().BeLessThan(ids[1]);
                ids[1].Should().BeLessThan(ids[0]);
            }
        }

        // get by id should return 1 
        [Test]
        public async Task GetByIdShouldReturnCorrectOrder()
        {
            // Arrange
            Order? order = _orders[1];

            // Act
            Order? ordersInDB = await _orderRepository.GetById(_orders[1].Id);

            // Assert
            ordersInDB.Should().Be(order);
        }

        // get by id should throw exception
        [Test]
        public async Task GetByIdShouldThrowNotFoundExceptionWhenOrderNotExists()
        {
            // Arrange
            string? type = "";
            int id = -99999;

            // Act
            try
            {
                await _orderRepository.GetById(id);
            }
            catch (Exception ex)
            {
                type = ex.GetType()?.FullName;
            }

            // Assert
            type.Should().Be("BillingAPI.Errors.NotFoundException");
        }

        // update should update
        [Test]
        public async Task UpdateShouldUpdate()
        {
            // Arrange
            Order? order = _orders[1];
            order.No = "gta";

            // Act
            await _orderRepository.Update(order);
            Order? updatedOrder = await _orderRepository.GetById(order.Id);


            // Assert
            updatedOrder.Should().Be(order);
        }

        // update should throw exception
        [Test]
        public async Task UpdateShouldThrowExceptionIfOrderNotExists()
        {
            // Arrange
            string? type = "";
            Order order = new()
            {
                Id = -9999,
                No = "gta"
            };

            // Act
            try
            {
                await _orderRepository.Update(order);
            }
            catch (Exception ex)
            {
                type = ex.GetType()?.FullName;
            }


            // Assert
            type.Should().Be("BillingAPI.Errors.NotFoundException");
        }

        // processing order with existing order no should throw badrequest exception
        [Test]
        public async Task ProcessingOrderWithExistingOrderNoThrowsBadRequestException()
        {
            // Arrange
            ProcessOrderDTO? processOrderDTO = new ProcessOrderDTO
            {
                Description = "hello",
                GatewayId = _gateways[0].Id,
                OrderNumber = _orders[0].No,
                PayableAmount = 40,
                UserId = _users[0].Id,
            };
            string? type = "";

            // Act
            try
            {
                await _orderRepository.ProcessNewOrder(processOrderDTO);
            }
            catch (Exception ex)
            {
                type = ex.GetType()?.FullName;
            }


            // Assert
            type.Should().Be("BillingAPI.Errors.BadRequestException");
        }
        // processing order with unexisting gateway should throw notfound exception
        [Test]
        public async Task ProcessingOrderWithUnexistingGatewayThrowsNotFoundException()
        {
            // Arrange
            ProcessOrderDTO? processOrderDTO = new ProcessOrderDTO
            {
                Description = "hello",
                GatewayId = -9999,
                OrderNumber = "hello",
                PayableAmount = 40,
                UserId = _users[0].Id,
            };
            string? type = "";

            // Act
            try
            {
                await _orderRepository.ProcessNewOrder(processOrderDTO);
            }
            catch (Exception ex)
            {
                type = ex.GetType()?.FullName;
            }


            // Assert
            type.Should().Be("BillingAPI.Errors.NotFoundException");
        }

        // processing order with unexisting user should throw notfound exception
        [Test]
        public async Task ProcessingOrderWithUnexistingUserThrowsNotFoundException()
        {
            // Arrange
            ProcessOrderDTO? processOrderDTO = new ProcessOrderDTO
            {
                Description = "hello",
                GatewayId = _gateways[0].Id,
                OrderNumber = "hello",
                PayableAmount = 40,
                UserId = -9999,
            };
            string? type = "";

            // Act
            try
            {
                await _orderRepository.ProcessNewOrder(processOrderDTO);
            }
            catch (Exception ex)
            {
                type = ex.GetType()?.FullName;
            }


            // Assert
            type.Should().Be("BillingAPI.Errors.NotFoundException");
        }

        // if user doesn't have enough balance when creating order, unsuccesfful payment should be created and badrequest exception
        //should be thrown
        [Test]
        public async Task ProcessingOrderWithNotEnoughBalanceThrowsBadRequestExceptionAndCreationUnsuccesfullPayment()
        {
            // Arrange
            ProcessOrderDTO? processOrderDTO = new ProcessOrderDTO
            {
                Description = "hello",
                GatewayId = _gateways[0].Id,
                OrderNumber = "hello",
                PayableAmount = 40000,
                UserId = _users[0].Id,
            };
            string? type = "";
            int oldCount = _payments.Count;

            // Act
            try
            {
                await _orderRepository.ProcessNewOrder(processOrderDTO);
            }
            catch (Exception ex)
            {
                type = ex.GetType()?.FullName;
            }
            await _dataContext.SaveChangesAsync();
            List<Payment>? newPayments = await _dataContext.Payments.OrderByDescending(p => p.Id).ToListAsync();
            Payment? newPayment = newPayments[0];

            // Assert
            type.Should().Be("BillingAPI.Errors.BadRequestException");
            newPayments.Count.Should().Be(oldCount + 1);
            newPayment.Amount.Should().Be(processOrderDTO.PayableAmount);
            newPayment.IsSuccessfull.Should().Be(false);
            newPayment.UserId.Should().Be(_users[0].Id);
        }

        // processing order with correct parameters should create a new order and return a correct receipt
        [Test]
        public async Task ProcessingOrderCreatesOrderAndPaymentAndDecreasesBalanceAndReturnsReceipt()
        {
            // Arrange
            ProcessOrderDTO? processOrderDTO = new ProcessOrderDTO
            {
                Description = "hello",
                GatewayId = _gateways[0].Id,
                OrderNumber = "hello",
                PayableAmount = 40,
                UserId = _users[0].Id,
            };
            ReceiptDTO? expectedReceipt = new ReceiptDTO
            {
                OrderNo = processOrderDTO.OrderNumber,
                PaidAmount = processOrderDTO.PayableAmount,
                UserEmail = _users[0].Email,
                UserName = _users[0].Name,
                UserSurname = _users[0].Surname
            };
            int oldOrderCount = _orders.Count;
            int oldPaymentCount = _payments.Count;
            int oldBalanceCount = _balances.Count;

            // Act
            ReceiptDTO? receipt = await _orderRepository.ProcessNewOrder(processOrderDTO);
            await _dataContext.SaveChangesAsync();
            IEnumerable<Order>? ordersInDb = await _orderRepository.GetAll("desc");
            expectedReceipt.Date = ordersInDb.Select(o => o).ToList()[0].CreatedAt.ToString();
            List<Payment>? paymentsInDb = await _dataContext.Payments.OrderByDescending(o => o.Id).ToListAsync();
            Payment? newPayment = paymentsInDb[0];
            List<Balance>? balancesInDb = await _dataContext.Balances.OrderByDescending(o => o.Id).ToListAsync();
            Balance? newBalance = balancesInDb[0];

            // Assert
            ordersInDb.Select(o => o).ToList().Count.Should().Be(oldOrderCount + 1);
            paymentsInDb.Count.Should().Be(oldPaymentCount + 1);
            balancesInDb.Count.Should().Be(oldBalanceCount + 1);
            newBalance.Amount.Should().Be(_balances[0].Amount - processOrderDTO.PayableAmount);
            newBalance.UserId.Should().Be(processOrderDTO.UserId);
            newPayment.IsSuccessfull.Should().Be(true);
            // below fails test even tho Should().Be() is not comparing the pointers ???
            //receipt.Should().Be(expectedReceipt);
            receipt.OrderNo.Should().Be(expectedReceipt.OrderNo);
            receipt.PaidAmount.Should().Be(expectedReceipt.PaidAmount);
            receipt.UserEmail.Should().Be(expectedReceipt.UserEmail);
            receipt.UserName.Should().Be(expectedReceipt.UserName);
            receipt.UserSurname.Should().Be(expectedReceipt.UserSurname);
            receipt.Date.Should().Be(expectedReceipt.Date);
        }
    }
}

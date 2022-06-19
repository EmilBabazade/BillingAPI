using BillingAPI.Data;
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
                _dataContext, new GatewayRepository(_dataContext), new UserRepository(_dataContext)
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
            await _orderRepository.Add(g);
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
    }
}

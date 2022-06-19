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
    public class GatewayRepositoryTests : BaseRepositoryTests
    {
        private GatewayRepository _gatewayRepository;

        [SetUp]
        public void SetupRepository()
        {
            _gatewayRepository = new GatewayRepository(_dataContext);
        }

        //// add should add
        [Test]
        public async Task AddingNewGatewayShouldAddNewGatewayToGatewaysTable()
        {
            // Arrange
            Gateway? g = new Gateway
            {
                No = "gt55"
            };

            // Act
            _gatewayRepository.Add(g);
            await _dataContext.SaveChangesAsync();
            Gateway? newGateway = await _dataContext.Gateways.FindAsync(g.Id);
            List<Gateway>? gatewaysInDb = await _dataContext.Gateways.ToListAsync();

            // Assert
            newGateway.Should().NotBeNull();
            gatewaysInDb.Count.Should().Be(_gateways.Count + 1);
            newGateway.Should().Be(g);
        }

        //// delete by id should delete
        [Test]
        public async Task DeleteByIdShouldLessenLengthBy1AndDelete()
        {
            // Arrange
            int deleteId = _gateways[0].Id;

            // Act
            _gatewayRepository.DeleteById(deleteId);
            await _dataContext.SaveChangesAsync();
            Gateway? deletedGateway = await _dataContext.Gateways.FindAsync(deleteId);
            List<Gateway>? gatewaysInDB = await _dataContext.Gateways.ToListAsync();

            // Assert
            deletedGateway.Should().BeNull();
            gatewaysInDB.Count.Should().Be(_gateways.Count - 1);
        }

        // delete by id should throw exception
        [Test]
        public void DeleteByIdShouldThrowNotFoundExceptionForNonExistentGateway()
        {
            // Arrange
            int deleteId = -99999;
            string? type = "";

            // Act
            try
            {
                _gatewayRepository.DeleteById(deleteId);
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
            int oldCount = _gateways.Count;

            // Act
            IEnumerable<Gateway>? gatewaysInDb = await _gatewayRepository.GetAll();


            // Assert
            gatewaysInDb.Select(b => b).ToList().Count.Should().Be(oldCount);
        }

        // get all asc should return ascending
        [Test]
        [TestCase("asc")]
        [TestCase("desc")]
        public async Task GetAllAscendingShouldReturnInAscendingOrderAndDescendingShouldReturnInDescendingOrder(string order)
        {
            // Arrange

            // Act
            IEnumerable<Gateway>? gatewaysInDb = await _gatewayRepository.GetAll(order);
            List<int>? ids = gatewaysInDb.Select(b => b.Id).ToList();


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
        public async Task GetByIdShouldReturnCorrectGateway()
        {
            // Arrange
            Gateway? gateway = _gateways[1];

            // Act
            Gateway? gatewaysInDb = await _gatewayRepository.GetById(_gateways[1].Id);

            // Assert
            gatewaysInDb.Should().Be(gateway);
        }

        // get by id should throw exception
        [Test]
        public async Task GetByIdShouldThrowNotFoundExceptionWhenGatewayNotExists()
        {
            // Arrange
            string? type = "";
            int id = -99999;

            // Act
            try
            {
                await _gatewayRepository.GetById(id);
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
            Gateway? gateway = _gateways[1];
            gateway.No = "gta";

            // Act
            await _gatewayRepository.Update(gateway);
            Gateway? updatedGateway = await _gatewayRepository.GetById(gateway.Id);


            // Assert
            updatedGateway.Should().Be(gateway);
        }

        // update should throw exception
        [Test]
        public async Task UpdateShouldThrowExceptionIfGatewayNotExists()
        {
            // Arrange
            string? type = "";
            Gateway gateway = new()
            {
                Id = -9999,
                No = "gta"
            };

            // Act
            try
            {
                await _gatewayRepository.Update(gateway);
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

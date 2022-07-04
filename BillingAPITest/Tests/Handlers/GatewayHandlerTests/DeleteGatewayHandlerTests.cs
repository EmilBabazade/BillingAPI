using BillingAPI.API.Gateway;
using BillingAPI.API.Gateway.Handlers;

using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BillingAPITest.Tests.Handlers.GatewayHandlerTests
{
    public class DeleteGatewayHandlerTests : BaseHandlerTests
    {
        private DeleteGatewayHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _handler = new DeleteGatewayHandler(_dataContext);
        }

        //// delete by id should delete
        [Test]
        public async Task DeleteByIdShouldLessenLengthBy1AndDelete()
        {
            // Arrange
            int deleteId = _gateways[0].Id;

            // Act
            await _handler.Handle(new DeleteGatewayCommand(deleteId), new CancellationToken());
            GatewayEntity? deletedGateway = await _dataContext.Gateways.FindAsync(deleteId);
            List<GatewayEntity>? gatewaysInDB = await _dataContext.Gateways.ToListAsync();

            // Assert
            deletedGateway.Should().BeNull();
            gatewaysInDB.Count.Should().Be(_gateways.Count - 1);
        }

        // delete by id should throw exception
        [Test]
        public async Task DeleteByIdShouldThrowNotFoundExceptionForNonExistentGateway()
        {
            // Arrange
            int deleteId = -99999;
            string? type = "";

            // Act
            try
            {
                await _handler.Handle(new DeleteGatewayCommand(deleteId), new CancellationToken());
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

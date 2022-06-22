using BillingAPI.DTOs;
using BillingAPI.Entities;
using BillingAPI.Mediatr;
using BillingAPI.Mediatr.Handlers.GatewayHandlers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BillingAPITest.Tests.Handlers.GatewayHandlerTests
{
    public class AddGatewayHandlerTests : BaseHandlerTests
    {
        private AddGatewayHandler _handler;

        [OneTimeSetUp]
        public void SetUp()
        {
            _handler = new AddGatewayHandler(_dataContext, _mapper);
        }

        // add should add
        [Test]
        public async Task AddingNewGatewayShouldAddNewGatewayToGatewaysTable()
        {
            // Arrange
            AddGatewayDTO? g = new AddGatewayDTO
            {
                No = "gt55"
            };

            // Act
            GatewayDTO? createdGateway = await _handler.Handle(new AddGatewayCommand(g), new CancellationToken());
            List<Gateway>? gatewaysInDb = await _dataContext.Gateways.ToListAsync();

            // Assert
            createdGateway.Should().NotBeNull();
            gatewaysInDb.Count.Should().Be(_gateways.Count + 1);
            createdGateway.No.Should().Be(g.No);
        }

        // add with existing no should throw exception
        [Test]
        public async Task AddingGatewayWithExistingGatewayNoShouldThrowBadRequestException()
        {
            // Arrange
            AddGatewayDTO? g = new AddGatewayDTO { No = "gt1" };
            string? type = "";

            // Act
            try
            {
                await _handler.Handle(new AddGatewayCommand(g), new CancellationToken());
            }
            catch (Exception ex)
            {
                type = ex.GetType()?.FullName;
            }


            // Assert
            type.Should().Be("BillingAPI.Errors.BadRequestException");
        }
    }
}

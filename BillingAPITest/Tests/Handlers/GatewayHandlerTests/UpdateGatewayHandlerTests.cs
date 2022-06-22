using BillingAPI.DTOs;
using BillingAPI.Mediatr;
using BillingAPI.Mediatr.Handlers.GatewayHandlers;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BillingAPITest.Tests.Handlers.GatewayHandlerTests
{
    public class UpdateGatewayHandlerTests : BaseHandlerTests
    {
        private UpdateGatewayHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _handler = new UpdateGatewayHandler(_dataContext, _mapper);
        }

        // update should update
        [Test]
        public async Task UpdateShouldUpdate()
        {
            // Arrange
            UpdateGatewayDTO? updateGatewayDTO = new UpdateGatewayDTO
            {
                Id = _gateways[0].Id,
                No = "gta"
            };

            // Act
            GatewayDTO? updatedGateway = await _handler.Handle(new UpdateGatewayCommand(updateGatewayDTO), new CancellationToken());

            // Assert
            updatedGateway.Id.Should().Be(updateGatewayDTO.Id);
            updatedGateway.No.Should().Be(updateGatewayDTO.No);
        }

        // update should throw exception
        [Test]
        public async Task UpdateShouldThrowExceptionIfGatewayNotExists()
        {
            // Arrange
            UpdateGatewayDTO? updateGatewayDTO = new UpdateGatewayDTO
            {
                Id = -99999,
                No = "gta"
            };
            string? type = "";

            // Act
            try
            {
                await _handler.Handle(new UpdateGatewayCommand(updateGatewayDTO), new CancellationToken());
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

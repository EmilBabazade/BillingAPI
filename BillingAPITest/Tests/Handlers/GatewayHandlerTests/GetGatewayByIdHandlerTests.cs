using BillingAPI.API.Gateway;
using BillingAPI.API.Gateway.DTOs;
using BillingAPI.API.Gateway.Handlers;

using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BillingAPITest.Tests.Handlers.GatewayHandlerTests
{
    public class GetGatewayByIdHandlerTests : BaseHandlerTests
    {
        private GetGatewayByIdHandler _handler;

        [OneTimeSetUp]
        public void SetUp()
        {
            _handler = new GetGatewayByIdHandler(_dataContext, _mapper);
        }

        // get by id should return 1 
        [Test]
        public async Task GetByIdShouldReturnCorrectGateway()
        {
            // Arrange
            GatewayDTO? gateway = _mapper.Map<GatewayDTO>(_gateways[1]);

            // Act
            GatewayDTO? gatewaysInDb = await _handler.Handle(new GetGatewayByIdQuery(_gateways[1].Id), new CancellationToken());

            // Assert
            gatewaysInDb.Should().BeEquivalentTo(gateway);
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
                await _handler.Handle(new GetGatewayByIdQuery(id), new CancellationToken());
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

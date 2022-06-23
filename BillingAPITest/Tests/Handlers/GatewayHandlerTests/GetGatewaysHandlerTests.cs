using BillingAPI.DTOs.Gateway;
using BillingAPI.Mediatr;
using BillingAPI.Mediatr.Handlers.GatewayHandlers;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BillingAPITest.Tests.Handlers.GatewayHandlerTests
{
    public class GetGatewaysHandlerTests : BaseHandlerTests
    {
        private GetGatewaysHandler _handler;

        [OneTimeSetUp]
        public void SetUp()
        {
            _handler = new GetGatewaysHandler(_dataContext, _mapper);
        }

        // get all should get all
        [Test]
        public async Task GetAllShouldReturnSameLengthAsInitialList()
        {
            // Arrange
            int oldCount = _gateways.Count;

            // Act
            IEnumerable<GatewayDTO> gatewaysInDb = await _handler.Handle(new GetGatewaysQuery(), new CancellationToken());


            // Assert
            gatewaysInDb.Select(b => b).ToList().Count.Should().Be(oldCount);
        }

        // get all asc should return ascending and desc should return descending
        [Test]
        [TestCase("asc")]
        [TestCase("desc")]
        public async Task GetAllAscendingShouldReturnInAscendingOrderAndDescendingShouldReturnInDescendingOrder(string order)
        {
            // Arrange

            // Act
            IEnumerable<GatewayDTO>? gatewaysInDb = await _handler.Handle(new GetGatewaysQuery(order), new CancellationToken());
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
    }
}

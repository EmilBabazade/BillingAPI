using BillingAPI.DTOs;
using BillingAPI.Mediatr;
using BillingAPI.Mediatr.Handlers.UserHandlers;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BillingAPITest.Tests.Handlers.UsersHandlerTests
{
    public class GetUsersHandlerTests : BaseHandlerTests
    {
        private GetUsersHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _handler = new GetUsersHandler(_dataContext, _mapper);
        }

        // get all should get all
        [Test]
        public async Task GetAllShouldReturnSameLengthAsInitialList()
        {
            // Arrange
            int oldCount = _users.Count;

            // Act
            IEnumerable<UserDTO>? usersInDB = await _handler.Handle(new GetUsersQuery(""), new CancellationToken());

            // Assert
            usersInDB.Select(b => b).ToList().Count.Should().Be(oldCount);
        }

        // get all asc should return ascending and desc should return descending
        [Test]
        [TestCase("asc")]
        [TestCase("desc")]
        public async Task GetAllAscendingShouldReturnInAscendingOrderAndDescendingShouldReturnInDescendingOrder(string order)
        {
            // Arrange

            // Act
            IEnumerable<UserDTO>? usersInDB = await _handler.Handle(new GetUsersQuery(order), new CancellationToken());
            List<int>? ids = usersInDB.Select(b => b.Id).ToList();


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

using BillingAPI.DTOs;
using BillingAPI.Entities;
using BillingAPI.Mediatr;
using BillingAPI.Mediatr.Handlers.UserHandlers;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BillingAPITest.Tests.Handlers.UsersHandlerTests
{
    public class GetUserByIdHandlerTests : BaseHandlerTests
    {
        private GetUserByIdHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _handler = new GetUserByIdHandler(_dataContext, _mapper);
        }

        // get by id should return 1 
        [Test]
        public async Task GetByIdShouldReturnCorrectUser()
        {
            // Arrange
            User? user = _users[1];

            // Act
            UserDTO userInDB = await _handler.Handle(new GetUserByİdQuery(user.Id), new CancellationToken());

            // Assert
            userInDB.Should().BeEquivalentTo(_mapper.Map<UserDTO>(user));
        }

        // get by id should throw exception
        [Test]
        public async Task GetByIdShouldThrowNotFoundExceptionWhenUserNotExists()
        {
            // Arrange
            string? type = "";
            int id = -99999;

            // Act
            try
            {
                await _handler.Handle(new GetUserByİdQuery(id), new CancellationToken());
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

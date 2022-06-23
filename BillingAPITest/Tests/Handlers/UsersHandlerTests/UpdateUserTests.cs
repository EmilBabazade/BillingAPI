using BillingAPI.DTOs.User;
using BillingAPI.Mediatr;
using BillingAPI.Mediatr.Handlers.UserHandlers;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BillingAPITest.Tests.Handlers.UsersHandlerTests
{
    public class UpdateUserTests : BaseHandlerTests
    {
        private UpdateUserHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _handler = new UpdateUserHandler(_dataContext, _mapper);
        }

        // update should update
        [Test]
        public async Task UpdateShouldUpdate()
        {
            // Arrange
            UpdateUserDTO? user = new UpdateUserDTO
            {
                Id = _users[1].Id,
                Email = "tommy@tommy124.23344",
                Name = "tom",
                Surname = "tommy"
            };

            // Act
            UserDTO? updatedUser = await _handler.Handle(new UpdateUserCommand(user), new CancellationToken());

            // Assert
            updatedUser.Id.Should().Be(user.Id);
            updatedUser.Email.Should().Be(user.Email);
            updatedUser.Name.Should().Be(user.Name);
            updatedUser.Surname.Should().Be(user.Surname);
        }

        // update should throw exception
        [Test]
        public async Task UpdateShouldThrowExceptionIfUserNotExists()
        {
            // Arrange
            UpdateUserDTO? user = new UpdateUserDTO
            {
                Id = -99999,
                Email = "tommy@tommy124.23344",
                Name = "tom",
                Surname = "tommy"
            };
            string? type = "";

            // Act
            try
            {
                await _handler.Handle(new UpdateUserCommand(user), new CancellationToken());
            }
            catch (Exception ex)
            {
                type = ex.GetType()?.FullName;
            }

            // Assert
            type.Should().Be("BillingAPI.Errors.NotFoundException");
        }

        // update should throw exception if email already exists
        [Test]
        public async Task UpdateThrowsBadRequestExceptionIfEmailAlreadyExists()
        {
            // Arrange
            UpdateUserDTO? user = new UpdateUserDTO
            {
                Id = _users[1].Id,
                Email = "tommy@tommy124.23344",
                Name = "tom",
                Surname = "tommy"
            };
            string? type = "";

            // Act
            await _handler.Handle(new UpdateUserCommand(user), new CancellationToken());
            user.Id = 2;
            try
            {
                await _handler.Handle(new UpdateUserCommand(user), new CancellationToken());
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

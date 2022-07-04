using BillingAPI.API.User;
using BillingAPI.API.User.Handlers;

using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BillingAPITest.Tests.Handlers.UsersHandlerTests
{
    public class DeleteUserHandlerTests : BaseHandlerTests
    {
        private DeleteUserHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _handler = new DeleteUserHandler(_dataContext);
        }

        // delete by id should delete
        [Test]
        public async Task DeleteByIdShouldLessenLengthBy1AndDelete()
        {
            // Arrange
            int deleteId = _users[0].Id;

            // Act
            await _handler.Handle(new DeleteUserCommand(deleteId), new CancellationToken());
            UserEntity? deletedUser = await _dataContext.Users.FindAsync(deleteId);
            List<UserEntity>? usersInDB = await _dataContext.Users.ToListAsync();

            // Assert
            deletedUser.Should().BeNull();
            usersInDB.Count.Should().Be(_users.Count - 1);
        }

        // delete by id should throw exception
        [Test]
        public async Task DeleteByIdShouldThrowNotFoundExceptionForNonExistentUser()
        {
            // Arrange
            int deleteId = -99999;
            string? type = "";

            // Act
            try
            {
                await _handler.Handle(new DeleteUserCommand(deleteId), new CancellationToken());
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

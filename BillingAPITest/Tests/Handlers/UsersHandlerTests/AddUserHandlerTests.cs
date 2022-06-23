using BillingAPI.DTOs.User;
using BillingAPI.Entities;
using BillingAPI.Mediatr;
using BillingAPI.Mediatr.Handlers.UserHandlers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BillingAPITest.Tests.Handlers.UsersHandlerTests
{
    class AddUserHandlerTests : BaseHandlerTests
    {
        private AddUserHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _handler = new AddUserHandler(_dataContext, _mapper);
        }

        // add should add
        [Test]
        public async Task AddingNewUserShouldAddNewUserToUsersTable()
        {
            // Arrange
            AddUserDTO? u = new AddUserDTO
            {
                Email = "bb@bb.com",
                Name = "bb",
                Surname = "dd"
            };

            // Act
            UserDTO? newUser = await _handler.Handle(new AddUserCommand(u), new CancellationToken());
            List<User> usersInDB = await _dataContext.Users.ToListAsync();

            // Assert
            newUser.Should().NotBeNull();
            usersInDB.Count.Should().Be(_users.Count + 1);
            newUser.Email.Should().BeEquivalentTo(u.Email);
        }

        // add with existing email should throw BadRequestException
        [Test]
        public async Task AddingNewUserWithExistingEmailShouldThrowException()
        {
            // Arrange
            AddUserDTO? u = new AddUserDTO
            {
                Email = "bb@bb.com",
                Name = "bb",
                Surname = "dd"
            };
            string? type = "";

            // Act
            await _handler.Handle(new AddUserCommand(u), new CancellationToken());
            try
            {
                await _handler.Handle(new AddUserCommand(u), new CancellationToken());
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

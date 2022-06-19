using BillingAPI.Data;
using BillingAPI.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BillingAPITest.Tests.Repository
{
    public class UserRepositoryTest : BaseRepositoryTests
    {
        private UserRepository _userRepository;

        [SetUp]
        public void SetupRepository()
        {
            _userRepository = new UserRepository(_dataContext);
        }

        // add should add
        [Test]
        public async Task AddingNewUserShouldAddNewUserToUsersTable()
        {
            // Arrange
            User? u = new User
            {
                Email = "bb@bb.com",
                Name = "bb",
                Surname = "dd"
            };

            // Act
            _userRepository.Add(u);
            await _dataContext.SaveChangesAsync();
            User? newUser = await _dataContext.Users.FindAsync(u.Id);
            List<User>? usersInDB = await _dataContext.Users.ToListAsync();

            // Assert
            newUser.Should().NotBeNull();
            usersInDB.Count.Should().Be(_users.Count + 1);
            newUser.Should().Be(u);
        }

        //// delete by id should delete
        [Test]
        public async Task DeleteByIdShouldLessenLengthBy1AndDelete()
        {
            // Arrange
            int deleteId = _users[0].Id;

            // Act
            _userRepository.DeleteById(deleteId);
            await _dataContext.SaveChangesAsync();
            User? deletedUser = await _dataContext.Users.FindAsync(deleteId);
            List<User>? usersInDB = await _dataContext.Users.ToListAsync();

            // Assert
            deletedUser.Should().BeNull();
            usersInDB.Count.Should().Be(_users.Count - 1);
        }

        // delete by id should throw exception
        [Test]
        public void DeleteByIdShouldThrowNotFoundExceptionForNonExistentUser()
        {
            // Arrange
            int deleteId = -99999;
            string? type = "";

            // Act
            try
            {
                _userRepository.DeleteById(deleteId);
            }
            catch (Exception ex)
            {
                type = ex.GetType()?.FullName;
            }

            // Assert
            type.Should().Be("BillingAPI.Errors.NotFoundException");
        }

        // get all should get all
        [Test]
        public async Task GetAllShouldReturnSameLengthAsInitialList()
        {
            // Arrange
            int oldCount = _users.Count;

            // Act
            IEnumerable<User>? usersInDB = await _userRepository.GetAll();

            // Assert
            usersInDB.Select(b => b).ToList().Count.Should().Be(oldCount);
        }

        // get all asc should return ascending
        [Test]
        [TestCase("asc")]
        [TestCase("desc")]
        public async Task GetAllAscendingShouldReturnInAscendingOrderAndDescendingShouldReturnInDescendingOrder(string order)
        {
            // Arrange

            // Act
            IEnumerable<User>? usersInDB = await _userRepository.GetAll(order);
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

        // get by id should return 1 
        [Test]
        public async Task GetByIdShouldReturnCorrectUser()
        {
            // Arrange
            User? user = _users[1];

            // Act
            User? usersInDB = await _userRepository.GetById(_users[1].Id);

            // Assert
            usersInDB.Should().Be(user);
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
                await _userRepository.GetById(id);
            }
            catch (Exception ex)
            {
                type = ex.GetType()?.FullName;
            }

            // Assert
            type.Should().Be("BillingAPI.Errors.NotFoundException");
        }

        // update should update
        [Test]
        public async Task UpdateShouldUpdate()
        {
            // Arrange
            User? user = _users[1];
            user.Name = "tommy";

            // Act
            await _userRepository.Update(user);
            User? updatedUser = await _userRepository.GetById(user.Id);

            // Assert
            updatedUser.Should().Be(user);
        }

        // update should throw exception
        [Test]
        public async Task UpdateShouldThrowExceptionIfUserNotExists()
        {
            // Arrange
            string? type = "";
            User? user = new User
            {
                Name = "Bob",
                Email = "bob@bob.bobobobobo",
                Surname = "tombobbore"
            };

            // Act
            try
            {
                await _userRepository.Update(user);
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

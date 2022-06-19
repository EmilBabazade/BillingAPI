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
    public class BalanceRepositoryTests : BaseRepositoryTests
    {
        private BalanceRepository _balanceRepository;

        [SetUp]
        public void SetupRepository()
        {
            _balanceRepository = new BalanceRepository(_dataContext);
        }

        // add should add
        [Test]
        public async Task AddingNewBalanceShouldAddNewBalanceToBalancesTable()
        {
            // Arrange
            Balance? b = new Balance
            {
                Amount = 123,
                UserId = _users[0].Id,
                PaymentId = _payments[0].Id
            };

            // Act
            await _balanceRepository.Add(b);
            await _dataContext.SaveChangesAsync();
            Balance? newBalance = await _dataContext.Balances.FindAsync(b.Id);
            List<Balance>? balancesInDb = await _dataContext.Balances.ToListAsync();

            // Assert
            newBalance.Should().NotBeNull();
            balancesInDb.Count.Should().Be(_balances.Count + 1);
            newBalance.Should().Be(b);
        }

        // delete by id should delete
        [Test]
        public async Task DeleteByIdShouldLessenLengthBy1AndDelete()
        {
            // Arrange
            int deleteId = _balances[0].Id;

            // Act
            _balanceRepository.DeleteById(deleteId);
            await _dataContext.SaveChangesAsync();
            Balance? deletedBalance = await _dataContext.Balances.FindAsync(deleteId);
            List<Balance>? balancesInDb = await _dataContext.Balances.ToListAsync();

            // Assert
            deletedBalance.Should().BeNull();
            balancesInDb.Count.Should().Be(_balances.Count - 1);
        }

        // delete by id should throw exception
        [Test]
        public void DeleteByIdShouldThrowNotFoundExceptionForNonExistentBalance()
        {
            // Arrange
            int deleteId = -99999;
            string? type = "";

            // Act
            try
            {
                _balanceRepository.DeleteById(deleteId);
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
            int oldCount = _balances.Count;

            // Act
            IEnumerable<Balance>? balancesInDb = await _balanceRepository.GetAll();


            // Assert
            balancesInDb.Select(b => b).ToList().Count.Should().Be(oldCount);
        }

        // get all asc should return ascending
        [Test]
        [TestCase("asc")]
        [TestCase("desc")]
        public async Task GetAllAscendingShouldReturnInAscendingOrderAndDescendingShouldReturnInDescendingOrder(string order)
        {
            // Arrange
            IEnumerable<Balance>? balances = await _balanceRepository.GetAll(order);

            // Act
            List<int>? ids = balances.Select(b => b.Id).ToList();


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
        public async Task GetByIdShouldReturnCorrectBalance()
        {
            // Arrange
            Balance? balance = _balances[1];

            // Act
            Balance? balanceInDb = await _balanceRepository.GetById(_balances[1].Id);

            // Assert
            balanceInDb.Should().Be(balance);
        }

        // get by id should throw exception
        [Test]
        public async Task GetByIdShouldThrowNotFoundExceptionWhenBalanceNotExists()
        {
            // Arrange
            string? type = "";
            int id = -99999;

            // Act
            try
            {
                await _balanceRepository.GetById(id);
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
            Balance? balance = _balances[1];
            balance.Amount = 0;

            // Act
            await _balanceRepository.Update(balance);
            Balance? updatedBalance = await _balanceRepository.GetById(balance.Id);


            // Assert
            updatedBalance.Should().Be(balance);
        }

        // update should throw exception
        [Test]
        public async Task UpdateShouldThrowExceptionIfBalanceNotExists()
        {
            // Arrange
            string? type = "";
            Balance balance = new()
            {
                Id = -9999,
                Amount = 324324
            };

            // Act
            try
            {
                await _balanceRepository.Update(balance);
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

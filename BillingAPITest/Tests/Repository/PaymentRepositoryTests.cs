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
    public class PaymentRepositoryTests : BaseRepositoryTests
    {
        private PaymentRepository _paymentRepository;

        [SetUp]
        public void SetupRepository()
        {
            _paymentRepository = new PaymentRepository(_dataContext);
        }

        //// add should add
        [Test]
        public async Task AddingNewPaymentShouldAddNewPaymentToPaymentsTable()
        {
            // Arrange
            Payment? p = new Payment
            {
                Amount = 133,
                Description = "sdfds",
                IsSuccessfull = true
            };

            // Act
            _paymentRepository.Add(p);
            await _dataContext.SaveChangesAsync();
            Payment? newPayment = await _dataContext.Payments.FindAsync(p.Id);
            System.Collections.Generic.List<Payment>? paymentsInDB = await _dataContext.Payments.ToListAsync();

            // Assert
            newPayment.Should().NotBeNull();
            paymentsInDB.Count.Should().Be(_payments.Count + 1);
            newPayment.Should().Be(p);
        }

        //// delete by id should delete
        [Test]
        public async Task DeleteByIdShouldLessenLengthBy1AndDelete()
        {
            // Arrange
            int deleteId = _payments[0].Id;

            // Act
            _paymentRepository.DeleteById(deleteId);
            await _dataContext.SaveChangesAsync();
            Payment? deletedPayment = await _dataContext.Payments.FindAsync(deleteId);
            System.Collections.Generic.List<Payment>? paymentsInDB = await _dataContext.Payments.ToListAsync();

            // Assert
            deletedPayment.Should().BeNull();
            paymentsInDB.Count.Should().Be(_payments.Count - 1);
        }

        // delete by id should throw exception
        [Test]
        public void DeleteByIdShouldThrowNotFoundExceptionForNonExistentPayment()
        {
            // Arrange
            int deleteId = -99999;
            string? type = "";

            // Act
            try
            {
                _paymentRepository.DeleteById(deleteId);
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
            int oldCount = _payments.Count;

            // Act
            IEnumerable<Payment>? paymentsInDB = await _paymentRepository.GetAll();

            // Assert
            paymentsInDB.Select(b => b).ToList().Count.Should().Be(oldCount);
        }

        // get all asc should return ascending
        [Test]
        [TestCase("asc")]
        [TestCase("desc")]
        public async Task GetAllAscendingShouldReturnInAscendingOrderAndDescendingShouldReturnInDescendingOrder(string order)
        {
            // Arrange

            // Act
            IEnumerable<Payment>? paymentsInDB = await _paymentRepository.GetAll(order);
            List<int>? ids = paymentsInDB.Select(b => b.Id).ToList();


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
        public async Task GetByIdShouldReturnCorrectPayment()
        {
            // Arrange
            Payment? payment = _payments[1];

            // Act
            Payment? paymentsInDB = await _paymentRepository.GetById(_payments[1].Id);

            // Assert
            paymentsInDB.Should().Be(payment);
        }

        // get by id should throw exception
        [Test]
        public async Task GetByIdShouldThrowNotFoundExceptionWhenPaymentNotExists()
        {
            // Arrange
            string? type = "";
            int id = -99999;

            // Act
            try
            {
                await _paymentRepository.GetById(id);
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
            Payment? payment = _payments[1];
            payment.Amount = 999999;

            // Act
            await _paymentRepository.Update(payment);
            Payment? updatedPayment = await _paymentRepository.GetById(payment.Id);


            // Assert
            updatedPayment.Should().Be(payment);
        }

        // update should throw exception
        [Test]
        public async Task UpdateShouldThrowExceptionIfPaymentNotExists()
        {
            // Arrange
            string? type = "";
            Payment payment = new()
            {
                Amount = 99999
            };

            // Act
            try
            {
                await _paymentRepository.Update(payment);
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

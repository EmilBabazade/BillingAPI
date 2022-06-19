using BillingAPI.Data;
using BillingAPI.Entities;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace BillingAPITest.Tests.Repository
{
    public class BalanceRepositoryTests : BaseRepositoryTests
    {
        private BalanceRepository _balanceRepository;

        [SetUp]
        public void SetupBalanceRepository()
        {
            _balanceRepository = new BalanceRepository(_dataContext);
        }

        [Test]
        public async Task TestTest()
        {
            // Arrange
            Balance? b = await _balanceRepository.GetById(1);
            Payment? p = await _dataContext.Payments.FindAsync(1);

            // Act

            // Assert
            b.PaymentId.Should().Be(p.Id);
        }
    }
}

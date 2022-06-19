using BillingAPI.Data.interfaces;
using BillingAPI.Entities;
using BillingAPI.Errors;
using Microsoft.EntityFrameworkCore;

namespace BillingAPI.Data
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly DataContext _dataContext;

        public PaymentRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task Add(Payment payment)
        {
            await _dataContext.Payments.AddAsync(payment);
        }

        public void DeleteById(int id)
        {
            Payment? payment = _dataContext.Payments.Find(id);
            if (payment == null) throw new NotFoundException("Payment not found");
            _dataContext.Payments.Remove(payment);
        }

        public async Task<IEnumerable<Payment>> GetAll(string order = "")
        {
            if (order == "asc")
            {
                return await _dataContext.Payments.OrderBy(b => b.Id).ToListAsync();
            }
            if (order == "desc")
            {
                return await _dataContext.Payments.OrderByDescending(b => b.Id).ToListAsync();
            }
            return await _dataContext.Payments.ToListAsync();
        }

        public async Task<Payment> GetById(int id)
        {
            Payment? payment = await _dataContext.Payments.FindAsync(id);
            if (payment == null) throw new NotFoundException("Payment not found");
            return payment;
        }

        public async Task Update(Payment payment)
        {
            if (!await CheckExists(payment.Id)) throw new NotFoundException("Payment not found");
            _dataContext.Update(payment);
        }

        private async Task<bool> CheckExists(int id)
        {
            return await _dataContext.Payments.AnyAsync(b => b.Id == id);
        }
    }
}

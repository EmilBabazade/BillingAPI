using BillingAPI.Data.interfaces;

namespace BillingAPI.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _dataContext;

        public UnitOfWork(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public IBalanceRepository BalanceRepository => new BalanceRepository(_dataContext);

        public IGatewayRepository GatewayRepository => new GatewayRepository(_dataContext);

        public IOrderRepository OrderRepository => new OrderRepository(_dataContext);

        public IPaymentRepository PaymentRepository => new PaymentRepository(_dataContext);

        public IUserRepository UserRepository => new UserRepository(_dataContext);

        public bool HasChanges()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveChanges()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }
    }
}

namespace BillingAPI.Data.interfaces
{
    public interface IUnitOfWork
    {
        public IBalanceRepository BalanceRepository { get; }
        public IGatewayRepository GatewayRepository { get; }
        public IOrderRepository OrderRepository { get; }
        public IPaymentRepository PaymentRepository { get; }
        public IUserRepository UserRepository { get; }
        public Task<bool> SaveChanges();
        public bool HasChanges();
    }
}

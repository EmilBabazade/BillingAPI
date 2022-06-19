using AutoMapper;
using BillingAPI.Data.interfaces;

namespace BillingAPI.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public UnitOfWork(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }
        public IBalanceRepository BalanceRepository => new BalanceRepository(_dataContext);

        public IGatewayRepository GatewayRepository => new GatewayRepository(_dataContext, _mapper);

        public IOrderRepository OrderRepository => new OrderRepository(_dataContext, GatewayRepository, UserRepository,
            BalanceRepository, PaymentRepository);

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

using AutoMapper;
using BillingAPI.Data;
using BillingAPI.DTOs.Balance;
using BillingAPI.Entities;
using BillingAPI.Errors;
using MediatR;

namespace BillingAPI.Mediatr.Handlers.BalanceHandlers
{
    public class GetBalanceHandler : IRequestHandler<GetBalanceQuery, BalanceDTO>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public GetBalanceHandler(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }
        public async Task<BalanceDTO> Handle(GetBalanceQuery request, CancellationToken cancellationToken)
        {
            Balance? balance = await _dataContext.Balances.FindAsync(request.id);
            if (balance == null)
                throw new NotFoundException("Balance not found");
            return _mapper.Map<BalanceDTO>(balance);
        }
    }
}

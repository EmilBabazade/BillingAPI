using AutoMapper;
using AutoMapper.QueryableExtensions;
using BillingAPI.API.Balance.DTOs;
using BillingAPI.Data;
using BillingAPI.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BillingAPI.API.Balance.Handlers
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
            BalanceDTO? balance = await _dataContext.Balances.ProjectTo<BalanceDTO>(
                        _mapper.ConfigurationProvider
                    ).SingleOrDefaultAsync(b => b.Id == request.id);
            if (balance == null)
                throw new NotFoundException("Balance not found");
            return balance;
        }
    }
}

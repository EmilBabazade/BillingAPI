using AutoMapper;
using AutoMapper.QueryableExtensions;
using BillingAPI.API.Balance.DTOs;
using BillingAPI.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BillingAPI.API.Balance.Handlers
{
    public class GetBalancesHandler : IRequestHandler<GetBalancesQuery, IEnumerable<BalanceDTO>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public GetBalancesHandler(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }
        public async Task<IEnumerable<BalanceDTO>> Handle(GetBalancesQuery request, CancellationToken cancellationToken)
        {
            IQueryable<BalanceEntity>? query = _dataContext.Balances.AsQueryable();
            query = FilterByUserId(request, query);
            query = OrderByAscending(request, query);
            query = OrderByDescending(request, query);
            return await query.ProjectTo<BalanceDTO>(
                        _mapper.ConfigurationProvider
                    ).ToListAsync();
        }

        private static IQueryable<BalanceEntity> OrderByDescending(GetBalancesQuery request, IQueryable<BalanceEntity> query)
        {
            if (request.order == "desc")
                query = query.OrderByDescending(b => b.Id);
            return query;
        }

        private static IQueryable<BalanceEntity> OrderByAscending(GetBalancesQuery request, IQueryable<BalanceEntity> query)
        {
            if (request.order == "asc")
                query = query.OrderBy(b => b.Id);
            return query;
        }

        private static IQueryable<BalanceEntity> FilterByUserId(GetBalancesQuery request, IQueryable<BalanceEntity> query)
        {
            if (request.userId != null)
                query = query.Where(b => b.UserId == request.userId);
            return query;
        }
    }
}

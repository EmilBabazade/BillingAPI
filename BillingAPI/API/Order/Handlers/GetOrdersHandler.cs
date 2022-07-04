using AutoMapper;
using AutoMapper.QueryableExtensions;
using BillingAPI.API.Order.DTOs;
using BillingAPI.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BillingAPI.API.Order.Handlers
{
    public class GetOrdersHandler : IRequestHandler<GetOrdersQuery, IEnumerable<OrderDTO>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public GetOrdersHandler(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }
        public async Task<IEnumerable<OrderDTO>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            IQueryable<OrderEntity>? query = _dataContext.Orders.AsQueryable();
            query = OrderByAscending(request, query);
            query = OrderByDescending(request, query);
            query = FilterByUserId(request, query);
            query = FilterByGatewayId(request, query);
            return await query.ProjectTo<OrderDTO>(_mapper.ConfigurationProvider).ToListAsync();
        }

        private static IQueryable<OrderEntity> FilterByGatewayId(GetOrdersQuery request, IQueryable<OrderEntity> query)
        {
            if (request.gatewayId != null)
                query = query.Where(o => o.GatewayId == request.gatewayId);
            return query;
        }

        private static IQueryable<OrderEntity> FilterByUserId(GetOrdersQuery request, IQueryable<OrderEntity> query)
        {
            if (request.userId != null)
                query = query.Where(o => o.UserId == request.userId);
            return query;
        }

        private static IQueryable<OrderEntity> OrderByDescending(GetOrdersQuery request, IQueryable<OrderEntity> query)
        {
            if (request.order == "desc")
                query = query.OrderByDescending(o => o.Id);
            return query;
        }

        private static IQueryable<OrderEntity> OrderByAscending(GetOrdersQuery request, IQueryable<OrderEntity> query)
        {
            if (request.order == "asc")
                query = query.OrderBy(o => o.Id);
            return query;
        }
    }
}

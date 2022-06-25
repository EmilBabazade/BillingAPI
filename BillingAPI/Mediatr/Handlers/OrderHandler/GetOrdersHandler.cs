using AutoMapper;
using AutoMapper.QueryableExtensions;
using BillingAPI.Data;
using BillingAPI.DTOs.Order;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BillingAPI.Mediatr.Handlers.OrderHandler
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
            IQueryable<Entities.Order>? query = _dataContext.Orders.AsQueryable();
            if (request.order == "asc")
            {
                query = query.OrderBy(o => o.Id);
            }
            if (request.order == "desc")
            {
                query = query.OrderByDescending(o => o.Id);
            }
            if (request.userId != null)
            {
                query = query.Where(o => o.UserId == request.userId);
            }
            if (request.gatewayId != null)
            {
                query = query.Where(o => o.GatewayId == request.gatewayId);
            }
            return await query.ProjectTo<OrderDTO>(_mapper.ConfigurationProvider).ToListAsync();
        }
    }
}

using AutoMapper;
using BillingAPI.Data;
using BillingAPI.DTOs.Order;
using BillingAPI.Entities;
using BillingAPI.Errors;
using MediatR;

namespace BillingAPI.Mediatr.Handlers.OrderHandler
{
    public class GetOrderHandler : IRequestHandler<GetOrderQuery, OrderDTO>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public GetOrderHandler(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }
        public async Task<OrderDTO> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            Order? order = await _dataContext.Orders.FindAsync(request.orderId);
            if (order == null)
                throw new NotFoundException("Order not found");
            return _mapper.Map<OrderDTO>(order);
        }
    }
}

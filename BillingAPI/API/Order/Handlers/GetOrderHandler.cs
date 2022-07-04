using AutoMapper;
using BillingAPI.API.Order.DTOs;
using BillingAPI.Data;
using BillingAPI.Errors;
using MediatR;

namespace BillingAPI.API.Order.Handlers
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
            OrderEntity? order = await _dataContext.Orders.FindAsync(request.orderId);
            if (order == null)
                throw new NotFoundException("Order not found");
            return _mapper.Map<OrderDTO>(order);
        }
    }
}

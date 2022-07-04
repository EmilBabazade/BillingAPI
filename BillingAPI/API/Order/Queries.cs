using BillingAPI.API.Order.DTOs;
using MediatR;

namespace BillingAPI.API.Order
{
    public record GetOrdersQuery(string order = "", int? userId = null, int? gatewayId = null) : IRequest<IEnumerable<OrderDTO>>;
    public record GetOrderQuery(int orderId) : IRequest<OrderDTO>;
}

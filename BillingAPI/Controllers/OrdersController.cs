using BillingAPI.DTOs.Order;
using BillingAPI.Mediatr;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BillingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all orders
        /// </summary>
        /// <param name="order">"asc" for ascending, "desc" for descending Id</param>
        /// <param name="userId">User id</param>
        /// <param name="gatewayId">Gateway id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrders(string? order = "", int? userId = null,
            int? gatewayId = null)
        {
            return Ok(await _mediator.Send(new GetOrdersQuery(order, userId, gatewayId)));
        }
    }
}

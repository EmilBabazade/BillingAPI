using BillingAPI.API.Payments.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BillingAPI.API.Payments
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all payments
        /// </summary>
        /// <param name="successfull">Whether payment has been successfull</param>
        /// <param name="userId">Paying user</param>
        /// <param name="order">"asc" for ascending, "desc" for descending id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentDTO>>> GetPayments(bool? successfull = null, int? userId = null, string? order = "")
        {
            return Ok(await _mediator.Send(new GetPaymentsQuery(successfull, userId, order)));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentDTO>> GetPayment(int id)
        {
            return Ok(await _mediator.Send(new GetPaymentQuery(id)));
        }
    }
}

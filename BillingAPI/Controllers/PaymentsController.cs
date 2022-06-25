using BillingAPI.DTOs.Payment;
using BillingAPI.Mediatr;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BillingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentDTO>>> GetPayments(bool? successfull = null, int? userId = null, string? order = "")
        {
            return Ok(await _mediator.Send(new GetPaymentsQuery(successfull, userId, order)));
        }
    }
}

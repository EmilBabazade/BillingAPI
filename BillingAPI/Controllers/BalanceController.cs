using BillingAPI.DTOs.Balance;
using BillingAPI.Mediatr;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BillingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BalanceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BalanceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all balances
        /// </summary>
        /// <param name="userId">Filter by user</param>
        /// <param name="order">"asc" for ascending, "desc" for descending</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BalanceDTO>>> GetBalances(int? userId = null, string? order = "")
        {
            return Ok(await _mediator.Send(new GetBalancesQuery(userId, order)));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BalanceDTO>> GetBalance(int id)
        {
            return Ok(await _mediator.Send(new GetBalanceQuery(id)));
        }

        [HttpPost]
        public async Task<ActionResult<BalanceDTO>> AddBalance(AddBalanceDTO addBalanceDTO)
        {
            return Ok(await _mediator.Send(new AddBalanceCommand(addBalanceDTO)));
        }
    }
}

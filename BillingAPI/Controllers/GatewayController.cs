using BillingAPI.DTOs.Gateway;
using BillingAPI.Mediatr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BillingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GatewayController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GatewayController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// get all gateways
        /// </summary>
        /// <param name="order">"asc" for ascending, "desc" for descending id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GatewayDTO>>> GetAll(string? order)
        {
            return Ok(await _mediator.Send(new GetGatewaysQuery(order)));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GatewayDTO>> GetOne(int id)
        {
            return await _mediator.Send(new GetGatewayByIdQuery(id));
        }

        [HttpPost]
        public async Task<ActionResult<GatewayDTO>> AddGateway(AddGatewayDTO addGatewayDTO)
        {
            return await _mediator.Send(new AddGatewayCommand(addGatewayDTO));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteGateway(int id)
        {
            await _mediator.Send(new DeleteGatewayCommand(id));
            return NoContent();
        }

        [HttpPut]
        public async Task<GatewayDTO> UpdateGateway(UpdateGatewayDTO updateGatewayDTO)
        {
            return await _mediator.Send(new UpdateGatewayCommand(updateGatewayDTO));
        }
    }
}

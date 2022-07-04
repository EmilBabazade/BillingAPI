using BillingAPI.API.Account.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BillingAPI.API.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            await _mediator.Send(new RegisterCommand(registerDTO));
            return Ok();
        }

        [HttpPost("Login")]
        public async Task<ActionResult<AccountDTO>> Login(LoginDTO loginDTO)
        {
            return Ok(await _mediator.Send(new LoginCommand(loginDTO)));
        }
    }
}

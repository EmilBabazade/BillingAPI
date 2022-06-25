using BillingAPI.DTOs.Account;
using BillingAPI.Mediatr;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BillingAPI.Controllers
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

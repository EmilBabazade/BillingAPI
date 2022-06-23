using BillingAPI.DTOs;
using BillingAPI.Mediatr;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BillingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IMediator _mediatr;

        public UserController(IMediator mediatr)
        {
            _mediatr = mediatr;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAll(string? order)
        {
            return Ok(await _mediatr.Send(new GetUsersQuery(order)));
        }
    }
}

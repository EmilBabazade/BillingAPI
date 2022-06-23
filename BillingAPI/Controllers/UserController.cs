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

        /// <summary>
        /// Get all users
        /// </summary>
        /// <param name="order">"asc" for ascending order, "desc" for descending order</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAll(string? order)
        {
            return Ok(await _mediatr.Send(new GetUsersQuery(order)));
        }

        // get one user
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetById(int id)
        {
            return Ok(await _mediatr.Send(new GetUserByİdQuery(id)));
        }

        // create user

        // update user

        // delete user
    }
}

using BillingAPI.DTOs.User;
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
        /// <param name="order">"asc" for ascending order, "desc" for descending id</param>
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
            return Ok(await _mediatr.Send(new GetUserByIdQuery(id)));
        }

        // create user
        [HttpPost]
        public async Task<ActionResult<UserDTO>> AddUser(AddUserDTO addUserDTO)
        {
            return Ok(await _mediatr.Send(new AddUserCommand(addUserDTO)));
        }

        // update user
        [HttpPut]
        public async Task<ActionResult<UserDTO>> EditUser(UpdateUserDTO updateUserDTO)
        {
            return Ok(await _mediatr.Send(new UpdateUserCommand(updateUserDTO)));
        }

        // delete user
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _mediatr.Send(new DeleteUserCommand(id));
            return NoContent();
        }
    }
}

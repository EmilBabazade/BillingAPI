using AutoMapper;
using BillingAPI.Data.interfaces;
using BillingAPI.DTOs;
using BillingAPI.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BillingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public UserController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAll(string? order)
        {
            IEnumerable<User> users = await _uow.UserRepository.GetAll(order);
            return Ok(_mapper.Map<IEnumerable<UserDTO>>(users));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetOne(int id)
        {
            User user = await _uow.UserRepository.GetById(id);
            return Ok(_mapper.Map<UserDTO>(user));
        }
    }
}

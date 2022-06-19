using AutoMapper;
using BillingAPI.Data.interfaces;
using BillingAPI.DTOs;
using BillingAPI.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BillingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GatewayController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GatewayController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GatewayDTO>>> GetAll(string? order)
        {
            IEnumerable<Gateway> gateways = await _uow.GatewayRepository.GetAll(order);
            return Ok(_mapper.Map<IEnumerable<GatewayDTO>>(gateways));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GatewayDTO>> GetOne(int id)
        {
            Gateway gateway = await _uow.GatewayRepository.GetById(id);
            return Ok(_mapper.Map<GatewayDTO>(gateway));
        }
    }
}

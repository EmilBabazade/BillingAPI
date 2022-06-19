using AutoMapper;
using BillingAPI.Data.interfaces;
using BillingAPI.DTOs;
using BillingAPI.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BillingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BalanceController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public BalanceController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BalanceDTO>>> GetAll(string order)
        {
            IEnumerable<Balance> balances = await _uow.BalanceRepository.GetAll(order);
            return Ok(_mapper.Map<IEnumerable<BalanceDTO>>(balances));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BalanceDTO>> GetOne(int id)
        {
            Balance balance = await _uow.BalanceRepository.GetById(id);
            return Ok(_mapper.Map<BalanceDTO>(balance));
        }
    }
}

using AutoMapper;
using BillingAPI.Data.interfaces;
using BillingAPI.DTOs;
using BillingAPI.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BillingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public PaymentController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<PaymentDTO>> GetAll(string? order)
        {
            IEnumerable<Payment> payments = await _uow.PaymentRepository.GetAll(order);
            return Ok(_mapper.Map<IEnumerable<PaymentDTO>>(payments));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentDTO>> GetOne(int id)
        {
            Payment payment = await _uow.PaymentRepository.GetById(id);
            return Ok(_mapper.Map<PaymentDTO>(payment));
        }
    }
}

using AutoMapper;
using BillingAPI.Data.interfaces;
using BillingAPI.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BillingAPI.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class BillingController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public BillingController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<ReceiptDTO>> ProcessOrder(ProcessOrderDTO processOrderDTO)
        {
            ReceiptDTO output = await _uow.OrderRepository.ProcessNewOrder(processOrderDTO);
            await _uow.SaveChanges();
            return Ok(output);
        }
    }
}

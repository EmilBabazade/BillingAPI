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
        public ActionResult<ReceiptDTO> ProcessOrder(ProcessOrderDTO processOrderDTO)
        {
            // TODO: create payment
            // TODO: create order
            // TODO: create balance
            // TODO: update gateway
            // TODO: update user
            throw new NotImplementedException();
        }
    }
}

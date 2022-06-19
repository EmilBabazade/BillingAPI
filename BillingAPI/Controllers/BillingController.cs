using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace BillingAPI.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class BillingController : ControllerBase
    {
        private readonly IMapper _mapper;

        public BillingController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult ProcessOrder()
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

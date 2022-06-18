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
    }
}

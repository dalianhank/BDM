using BDM.Lambda.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BDM.Lambda.Controllers
{
    [Route("v1/BrokerList")]
    [ApiController]
    public class BrokerController : ControllerBase
    {
        private readonly IBrokerService _brokerService;

        public BrokerController(IBrokerService brokerService)
        {
            _brokerService = brokerService;
        }

        [HttpGet] 
        //[Authorize]       
        public IActionResult GetBrokerList()
        {
            var result = _brokerService.GetBrokerList();
            return Ok(result);
        }

    }
}
using ViewObj = BDM.Lambda.Model;
using BDM.Lambda.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BDM.Lambda.Controllers
{
    //[Route("v1/BrokerList")]
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
        [Route("/Clients/{clientName}")]       
        public IActionResult GetBrokerList(string clientName)
        {
            var result = _brokerService.GetBrokerList(clientName);
            return Ok(result);
        }
        [HttpGet] 
        //[Authorize]
        [Route("/Clients/{clientName}/NPN/{npn}")]       
        public IActionResult GetBrokeByClientNPN(string clientName, string npn)
        {
            var result = _brokerService.GetBrokerByClientNPN(clientName, npn);
            return Ok(result);
        }

        [HttpPost]
        [Route("/Clients/{clientName}/NPN/{npn}")] 
        public IActionResult CreateBroker(ViewObj.Broker broker)
        {
            _brokerService.AddBrokerByClientNPN(broker.ClientName, broker.NPN, broker);
            return Ok();
        }

        [HttpPut]
        [Route("/Clients/{clientName}/NPN/{npn}")] 
        public IActionResult UpdateBroker(ViewObj.Broker broker)
        {
            _brokerService.UpdateBrokerByClientNPN(broker.ClientName, broker.NPN, broker);
            return Ok();
        }

        [HttpDelete]
        [Route("/Clients/{clientName}/NPN/{npn}")] 
        public IActionResult DeleteBroker(string clientName, string npn)
        {
            _brokerService.DeleteBrokerByClientNPN(clientName, npn);
            return Ok();
        }
    }
}
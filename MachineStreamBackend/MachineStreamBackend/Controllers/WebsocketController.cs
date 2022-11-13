using MachineStreamBackend.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MachineStreamBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebsocketController : Controller
    {
        // GET: api/<WebsocketController>
        [HttpGet]
        public List<MachineStreamResultPayload> Get()
        {
            return WebsocketService.GetPayloadMessages();
        }

        [HttpGet("{id}")]
        public List<MachineStreamResultPayload> GetMachine(string id)
        {
            return WebsocketService.GetPayloadMessagesByMachine(id);
        }

        [HttpGet("latest/{id}")]
        public MachineStreamResultPayload GetLatestMachine(string id)
        {
            return WebsocketService.GetLatestPayloadMessagesByMachine(id);
        }

        [HttpGet("status/{id}")]
        public string GetMachineStatus(string id)
        {
            return WebsocketService.GetLatestPayloadMessagesByMachine(id).Status;
        }
    }
}

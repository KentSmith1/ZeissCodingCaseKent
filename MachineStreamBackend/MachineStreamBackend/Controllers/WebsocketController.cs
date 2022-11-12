using MachineStreamBackend.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MachineStreamBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebsocketController : ControllerBase
    {
        // GET: api/<WebsocketController>
        [HttpGet]
        public List<MachineStreamResult> Get()
        {
            return WebsocketService.GetMachineStreamMessages();
        }

        [HttpGet("{id}")]
        public List<MachineStreamResult> GetMachine(string id)
        {
            return WebsocketService.GetMachineStreamMessagesByMachine(id);
        }

        [HttpGet("status/{id}")]
        public string GetMachineStatus(string id)
        {
            return WebsocketService.GetMachineStatus(id);
        }

        //// GET api/<WebsocketController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<WebsocketController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<WebsocketController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<WebsocketController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}

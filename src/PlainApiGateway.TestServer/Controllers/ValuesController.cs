using Microsoft.AspNetCore.Mvc;

using PlainApiGateway.TestServer.ViewModel;

namespace PlainApiGateway.TestServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ResponseViewModel content = new ResponseViewModel
        {
            Id = 1,
            Name = "Name"
        };

        [HttpGet]
        public IActionResult Get()
        {
            return this.Ok("Test server is up and running");
        }

        [HttpGet("loadxml")]
        [Produces(ContentType.Xml)]
        public IActionResult GetXmlLoad()
        {
            return this.Ok(this.content);
        }

        [HttpGet("loadjson")]
        [Produces(ContentType.Json)]
        public IActionResult GetJsonLoad()
        {
            return this.Ok(this.content);
        }

        [HttpPost]
        public IActionResult Post()
        {
            return this.Ok(this.content);
        }
    }
}
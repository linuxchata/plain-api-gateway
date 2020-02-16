using Microsoft.AspNetCore.Mvc;

namespace PlainApiGateway.TestServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return this.Ok("Test server is up and running");
        }

        [HttpGet("loadxml")]
        [Produces(ContentType.Xml)]
        public IActionResult GetXmlLoad()
        {
            return this.Ok("Load");
        }

        [HttpGet("loadjson")]
        [Produces(ContentType.Json)]
        public IActionResult GetJsonLoad()
        {
            return this.Ok("Load");
        }
    }
}
using Microsoft.AspNetCore.Mvc;

using PlainApiGateway.TestServer.ViewModel;

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

        [HttpPost]
        public IActionResult Post()
        {
            var content = new ResponseViewModel
            {
                Id = 1,
                Name = "Name"
            };

            return this.Ok(content);
        }
    }
}
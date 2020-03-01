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

        [HttpGet("statuscode/{statusCode}")]
        public IActionResult GetStatusCode([FromRoute]int statusCode)
        {
            return this.StatusCode(statusCode);
        }

        [HttpGet("xml")]
        [Produces(ContentType.Xml)]
        public IActionResult GetXml()
        {
            return this.Ok(this.content);
        }

        [HttpGet("json")]
        [Produces(ContentType.Json)]
        public IActionResult GetJson()
        {
            return this.Ok(this.content);
        }

        [HttpPost]
        public IActionResult Post()
        {
            return this.Ok(this.content);
        }

        [HttpPut]
        public IActionResult Put()
        {
            return this.NoContent();
        }

        [HttpDelete]
        public IActionResult Delete()
        {
            return this.NoContent();
        }
    }
}
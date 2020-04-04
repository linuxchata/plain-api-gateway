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

        [HttpGet("statuscode/{statusCode}")]
        public IActionResult GetStatusCode([FromRoute]int statusCode)
        {
            return this.StatusCode(statusCode);
        }
    }
}
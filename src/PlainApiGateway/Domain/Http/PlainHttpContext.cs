using System.Net.Http;

namespace PlainApiGateway.Domain.Http
{
    public sealed class PlainHttpContext
    {
        public HttpResponseMessage Response { get; set; }
    }
}

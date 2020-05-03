using System.Net.Http;

namespace PlainApiGateway.Domain.Entity.Http
{
    public sealed class PlainHttpContext
    {
        public HttpResponseMessage Response { get; set; }
    }
}

using System.Net.Http;

namespace PlainApiGateway.Domain.Entity
{
    public sealed class PlainHttpContext
    {
        public HttpResponseMessage Response { get; set; }
    }
}

using System.Net.Http;

namespace PlainApiGateway.Domain.Http
{
    public sealed class PlainHttpContext : ValueObject
    {
        public HttpResponseMessage Response { get; private set; }

        public void SetResponse(HttpResponseMessage response)
        {
            this.Response = response;
        }
    }
}

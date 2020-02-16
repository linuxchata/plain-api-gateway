using System.Net.Http;

namespace PlainApiGateway.Context
{
    public sealed class PlainContext
    {
        public HttpResponseMessage Response { get; set; }
    }
}

using Microsoft.AspNetCore.Http;

using PlainApiGateway.Context;

namespace PlainApiGateway.Provider
{
    public sealed class HttpRequestProvider : IHttpRequestProvider
    {
        public RequestContext Create(HttpRequest httpRequest)
        {
            var request = new RequestContext
            {
                Headers = httpRequest.Headers,
                Method = httpRequest.Method,
                Scheme = httpRequest.Scheme,
                Host = httpRequest.Host.Host,
                Port = httpRequest.Host.Port ?? 80,
                Path = httpRequest.Path,
                QueryString = httpRequest.QueryString.Value ?? string.Empty
            };

            return request;
        }
    }
}

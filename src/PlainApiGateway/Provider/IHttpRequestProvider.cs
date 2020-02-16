using Microsoft.AspNetCore.Http;

using PlainApiGateway.Context;

namespace PlainApiGateway.Provider
{
    public interface IHttpRequestProvider
    {
        RequestContext Create(HttpRequest httpRequest);
    }
}
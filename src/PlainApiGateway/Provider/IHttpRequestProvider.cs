using Microsoft.AspNetCore.Http;

using PlainApiGateway.Context;

namespace PlainApiGateway.Provider
{
    public interface IHttpRequestProvider
    {
        /// <summary>
        /// Creates request context
        /// </summary>
        /// <param name="httpRequest">HTTP request</param>
        /// <returns>Returns request context</returns>
        RequestContext Create(HttpRequest httpRequest);
    }
}
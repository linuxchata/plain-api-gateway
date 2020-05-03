using Microsoft.AspNetCore.Http;

using PlainApiGateway.Domain.Entity;

namespace PlainApiGateway.Provider
{
    public interface IHttpRequestProvider
    {
        /// <summary>
        /// Creates plain HTTP request
        /// </summary>
        /// <param name="httpRequest">HTTP request</param>
        /// <returns>Returns plain HTTP request</returns>
        PlainHttpRequest Create(HttpRequest httpRequest);
    }
}
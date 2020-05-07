using Microsoft.AspNetCore.Http;

using PlainApiGateway.Domain.Entity.Http;

namespace PlainApiGateway.Domain.Factory.Http
{
    public interface IPlainHttpRequestFactory
    {
        /// <summary>
        /// Creates plain HTTP request
        /// </summary>
        /// <param name="httpRequest">HTTP request</param>
        /// <returns>Returns plain HTTP request</returns>
        PlainHttpRequest Create(HttpRequest httpRequest);
    }
}
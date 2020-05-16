using Microsoft.AspNetCore.Http;

namespace PlainApiGateway.Domain.Http.Factory
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
using Microsoft.AspNetCore.Http;

using PlainApiGateway.Configuration;

namespace PlainApiGateway.Domain.Http.Factory
{
    public interface IPlainHttpRequestFactory
    {
        /// <summary>
        /// Creates plain HTTP request
        /// </summary>
        /// <param name="method">HTTP method</param>
        /// <param name="path">URL path</param>
        /// <param name="queryString">URL query string</param>
        /// <param name="headers">HTTP request headers</param>
        /// <param name="timeoutInSeconds">Timeout in seconds</param>
        /// <param name="routeConfiguration">Route configuration</param>
        /// <returns>The plain HTTP request</returns>
        PlainHttpRequest Create(
            string method,
            string path,
            string queryString,
            IHeaderDictionary headers,
            ushort? timeoutInSeconds,
            PlainRouteConfiguration routeConfiguration);
    }
}
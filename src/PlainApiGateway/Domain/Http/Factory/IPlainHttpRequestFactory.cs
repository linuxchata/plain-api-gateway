using Microsoft.AspNetCore.Http;

using PlainApiGateway.Configuration;

namespace PlainApiGateway.Domain.Http.Factory
{
    public interface IPlainHttpRequestFactory
    {
        /// <summary>
        /// Creates plain HTTP request
        /// </summary>
        /// <param name="httpRequest">HTTP request</param>
        /// <param name="routeConfiguration">Route configuration</param>
        /// <param name="timeoutInSeconds">Timeout in seconds</param>
        /// <returns>Returns plain HTTP request</returns>
        PlainHttpRequest Create(HttpRequest httpRequest, PlainRouteConfiguration routeConfiguration, ushort? timeoutInSeconds);
    }
}
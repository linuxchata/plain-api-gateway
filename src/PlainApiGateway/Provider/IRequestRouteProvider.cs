using System.Collections.Generic;

using Microsoft.AspNetCore.Http;

using PlainApiGateway.Configuration;

namespace PlainApiGateway.Provider
{
    public interface IRequestRouteProvider
    {
        /// <summary>
        /// Get route configuration for the request
        /// </summary>
        /// <param name="routes">List of routes</param>
        /// <param name="httpRequest">HTTP request</param>
        /// <returns>Returns route configuration for the request</returns>
        PlainRouteConfiguration GetTargetRoute(List<PlainRouteConfiguration> routes, HttpRequest httpRequest);
    }
}
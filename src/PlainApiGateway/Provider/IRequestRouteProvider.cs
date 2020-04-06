using System.Collections.Generic;

using Microsoft.AspNetCore.Http;

using PlainApiGateway.Configuration;

namespace PlainApiGateway.Provider
{
    public interface IRequestRouteProvider
    {
        /// <summary>
        /// Get target route
        /// </summary>
        /// <param name="routes">List of routes</param>
        /// <param name="httpRequest">HTTP request</param>
        /// <returns>Returns target route</returns>
        PlainRouteTargetConfiguration GetTargetRoute(List<PlainRouteConfiguration> routes, HttpRequest httpRequest);
    }
}
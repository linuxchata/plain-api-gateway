using System.Collections.Generic;

using Microsoft.AspNetCore.Http;

namespace PlainApiGateway.Provider.Configuration
{
    public interface IPlainRouteConfigurationProvider
    {
        /// <summary>
        /// Gets matching route configuration for the request
        /// </summary>
        /// <param name="routes">List of routes</param>
        /// <param name="httpRequest">HTTP request</param>
        /// <returns>Returns matching route configuration for the request</returns>
        Domain.Entity.Configuration.PlainRouteConfiguration GetMatchingRouteConfiguration(List<Domain.Entity.Configuration.PlainRouteConfiguration> routes, HttpRequest httpRequest);
    }
}
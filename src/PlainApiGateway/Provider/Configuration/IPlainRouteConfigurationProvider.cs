using System.Collections.Generic;

using Microsoft.AspNetCore.Http;

using PlainApiGateway.Configuration;

namespace PlainApiGateway.Provider.Configuration
{
    public interface IPlainRouteConfigurationProvider
    {
        /// <summary>
        /// Gets matching plain route configuration for the request
        /// </summary>
        /// <param name="routes">List of route configurations</param>
        /// <param name="httpRequest">HTTP request</param>
        /// <returns>Returns matching plain route configuration for the request</returns>
        PlainRouteConfiguration GetMatching(List<PlainRouteConfiguration> routes, HttpRequest httpRequest);
    }
}
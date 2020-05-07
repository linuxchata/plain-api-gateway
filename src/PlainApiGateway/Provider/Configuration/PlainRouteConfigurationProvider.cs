using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Http;

using PlainApiGateway.Domain.Entity.Configuration;
using PlainApiGateway.Helper;

namespace PlainApiGateway.Provider.Configuration
{
    public sealed class PlainRouteConfigurationProvider : IPlainRouteConfigurationProvider
    {
        public PlainRouteConfiguration GetMatchingRouteConfiguration(
            List<PlainRouteConfiguration> routes,
            HttpRequest httpRequest)
        {
            if (routes == null)
            {
                throw new ArgumentNullException(nameof(routes));
            }

            if (httpRequest == null)
            {
                throw new ArgumentNullException(nameof(httpRequest));
            }

            var routeConfiguration = GetRouteConfiguration(routes, httpRequest);
            if (routeConfiguration == null)
            {
                return null;
            }

            if (!IsHttpMethodAllowed(httpRequest.Method, routeConfiguration.Source.HttpMethods))
            {
                return null;
            }

            if (!routeConfiguration.Target.Addresses?.Any() ?? true)
            {
                return null;
            }

            return routeConfiguration;
        }

        private static PlainRouteConfiguration GetRouteConfiguration(List<PlainRouteConfiguration> routes, HttpRequest httpRequest)
        {
            return routes.FirstOrDefault(a => PathFounderHelper.IsMatch(a.Source.PathTemplate, httpRequest.Path));
        }

        private static bool IsHttpMethodAllowed(string httpRequestMethod, string[] httpMethods)
        {
            return httpMethods.Any(a => string.Equals(a, httpRequestMethod, StringComparison.OrdinalIgnoreCase));
        }
    }
}
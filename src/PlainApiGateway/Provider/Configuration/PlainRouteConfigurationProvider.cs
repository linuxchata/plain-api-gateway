using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;

using PlainApiGateway.Configuration;
using PlainApiGateway.Helper;

namespace PlainApiGateway.Provider.Configuration
{
    public sealed class PlainRouteConfigurationProvider : IPlainRouteConfigurationProvider
    {
        private readonly ILogger logger;

        public PlainRouteConfigurationProvider(ILoggerFactory logFactory)
        {
            this.logger = logFactory.CreateLogger<PlainRouteConfigurationProvider>();
        }

        public PlainRouteConfiguration GetMatching(List<PlainRouteConfiguration> routes, HttpRequest httpRequest)
        {
            if (routes is null)
            {
                throw new ArgumentNullException(nameof(routes));
            }

            if (httpRequest is null)
            {
                throw new ArgumentNullException(nameof(httpRequest));
            }

            var routeConfiguration = GetRouteConfiguration(routes, httpRequest);
            if (routeConfiguration is null)
            {
                this.logger.LogWarning(
                    "No route configuration was found for {method} request to URL {url}",
                    httpRequest.Method,
                    httpRequest.GetDisplayUrl());

                return null;
            }

            if (!IsHttpMethodAllowed(httpRequest.Method, routeConfiguration.Source.HttpMethods))
            {
                this.logger.LogWarning(
                    "No allowed HTTP methods were found for {method} request to URL {url}",
                    httpRequest.Method,
                    httpRequest.GetDisplayUrl());

                return null;
            }

            if (!routeConfiguration.Target.Addresses?.Any() ?? true)
            {
                this.logger.LogWarning(
                    "No target addresses were found for {method} request to URL {url}",
                    httpRequest.Method,
                    httpRequest.GetDisplayUrl());

                return null;
            }

            this.logger.LogDebug(
                "Route configuration has been found for {method} request to URL {url}",
                httpRequest.Method,
                httpRequest.GetDisplayUrl());

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
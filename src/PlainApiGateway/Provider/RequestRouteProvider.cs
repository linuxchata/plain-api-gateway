using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.AspNetCore.Http;

using PlainApiGateway.Configuration;
using PlainApiGateway.Constant;

namespace PlainApiGateway.Provider
{
    public sealed class RequestRouteProvider : IRequestRouteProvider
    {
        public PlainRouteTargetConfiguration GetTargetRoute(List<PlainRouteConfiguration> routes, HttpRequest httpRequest)
        {
            if (routes == null)
            {
                throw new ArgumentNullException(nameof(routes));
            }

            if (httpRequest == null)
            {
                throw new ArgumentNullException(nameof(httpRequest));
            }

            var routeConfiguration = routes.FirstOrDefault(a => PathMatcher(a.Source.Path, httpRequest.Path));
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

            return routeConfiguration.Target;
        }

        private static bool PathMatcher(string sourcePath, PathString requestPath)
        {
            var matches = Regex.Matches(sourcePath, RoutePath.Any, RegexOptions.Compiled);
            if (matches.Count == 0)
            {
                return sourcePath == requestPath;
            }

            if (matches.Count == 1 && matches[0].Success)
            {
                return true;
            }

            throw new ArgumentException($"Source path {sourcePath} is invalid");
        }

        private static bool IsHttpMethodAllowed(string httpRequestMethod, string[] httpMethods)
        {
            return httpMethods.Any(a => string.Equals(a, httpRequestMethod, StringComparison.OrdinalIgnoreCase));
        }
    }
}
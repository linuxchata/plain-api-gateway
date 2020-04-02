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

            var routeConfiguration = routes.FirstOrDefault(a => PathMatcher(a, httpRequest.Path));
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

        private static bool PathMatcher(PlainRouteConfiguration c, PathString path)
        {
            var matches = Regex.Matches(c.Source.Path, RoutePath.Any, RegexOptions.Compiled);
            if (matches.Count == 0)
            {
                return c.Source.Path == path;
            }

            if (matches.Count == 1 && matches[0].Success)
            {
                return true;
            }

            throw new ArgumentException($"Source path {c.Source.Path} is invalid");
        }

        private static bool IsHttpMethodAllowed(string httpRequestMethod, string[] htpMethods)
        {
            return htpMethods.Any(a => string.Equals(a, httpRequestMethod, StringComparison.OrdinalIgnoreCase));
        }
    }
}
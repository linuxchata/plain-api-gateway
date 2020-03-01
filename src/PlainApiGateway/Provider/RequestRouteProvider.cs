using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.AspNetCore.Http;

using PlainApiGateway.Configuration;
using PlainApiGateway.Constants;

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

            var route = routes.FirstOrDefault(a => PathMatcher(a, httpRequest));
            if (route == null)
            {
                return null;
            }

            if (!IsHttpMethodAllowed(httpRequest, route))
            {
                return null;
            }

            if (!route.Target.Addresses?.Any() ?? true)
            {
                return null;
            }

            return route.Target;
        }

        private static bool PathMatcher(PlainRouteConfiguration c, HttpRequest httpRequest)
        {
            var matches = Regex.Matches(c.Source.Path, RoutePath.Any, RegexOptions.Compiled);
            if (matches.Count == 0)
            {
                return c.Source.Path == httpRequest.Path;
            }

            if (matches.Count == 1 && matches[0].Success)
            {
                return true;
            }

            throw new ArgumentException($"Source path {c.Source.Path} is invalid");
        }

        private static bool IsHttpMethodAllowed(HttpRequest httpRequest, PlainRouteConfiguration route)
        {
            return route.Source.HttpMethods.Any(a => string.Equals(a, httpRequest.Method, StringComparison.OrdinalIgnoreCase));
        }
    }
}
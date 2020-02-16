using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Http;

using PlainApiGateway.Configuration;

namespace PlainApiGateway.Provider
{
    public sealed class RequestRouteProvider : IRequestRouteProvider
    {
        public PlainRouteTargetConfiguration GetTargetRoute(List<PlainRouteConfiguration> routes, HttpRequest httpRequest)
        {
            var route = routes.FirstOrDefault(a => a.Source.Path == httpRequest.Path);

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

        private static bool IsHttpMethodAllowed(HttpRequest httpRequest, PlainRouteConfiguration route)
        {
            return route.Source.HttpMethods.Any(a => string.Equals(a, httpRequest.Method, StringComparison.OrdinalIgnoreCase));
        }
    }
}
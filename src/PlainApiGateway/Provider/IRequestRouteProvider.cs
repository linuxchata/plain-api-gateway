using System.Collections.Generic;

using Microsoft.AspNetCore.Http;

using PlainApiGateway.Configuration;

namespace PlainApiGateway.Provider
{
    public interface IRequestRouteProvider
    {
        PlainRouteTargetConfiguration GetTargetRoute(List<PlainRouteConfiguration> routes, HttpRequest httpRequest);
    }
}
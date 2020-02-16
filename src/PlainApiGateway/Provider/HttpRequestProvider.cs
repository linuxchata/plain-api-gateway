using System.Linq;

using Microsoft.AspNetCore.Http;

using PlainApiGateway.Context;
using PlainApiGateway.Repository;

namespace PlainApiGateway.Provider
{
    public sealed class HttpRequestProvider : IHttpRequestProvider
    {
        private readonly IPlainConfigurationRepository configurationRepository;

        private readonly IRequestRouteProvider requestRouteProvider;

        public HttpRequestProvider(
            IPlainConfigurationRepository configurationRepository, 
            IRequestRouteProvider requestRouteProvider)
        {
            this.configurationRepository = configurationRepository;
            this.requestRouteProvider = requestRouteProvider;
        }

        public RequestContext Create(HttpRequest httpRequest)
        {
            var configuration = this.configurationRepository.Get();

            var routeTarget = this.requestRouteProvider.GetTargetRoute(configuration.Routes, httpRequest);
            if (routeTarget == null)
            {
                return null;
            }

            var address = routeTarget.Addresses.First();

            var request = new RequestContext
            {
                Headers = httpRequest.Headers,
                Method = httpRequest.Method,
                Scheme = routeTarget.Scheme,
                Host = address.Host,
                Port = address.Port,
                Path = routeTarget.Path,
                QueryString = httpRequest.QueryString.Value ?? string.Empty,
                // ReSharper disable once PossibleInvalidOperationException
                TimeoutInSeconds = configuration.TimeoutInSeconds.Value
            };

            return request;
        }
    }
}

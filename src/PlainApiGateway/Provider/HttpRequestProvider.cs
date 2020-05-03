using System;
using System.Linq;

using Microsoft.AspNetCore.Http;

using PlainApiGateway.Domain.Entity.Http;
using PlainApiGateway.Repository;

namespace PlainApiGateway.Provider
{
    public sealed class HttpRequestProvider : IHttpRequestProvider
    {
        private readonly IPlainConfigurationRepository configurationRepository;

        private readonly IRequestRouteProvider requestRouteProvider;

        private readonly IRequestPathProvider requestPathProvider;

        public HttpRequestProvider(
            IPlainConfigurationRepository configurationRepository,
            IRequestRouteProvider requestRouteProvider,
            IRequestPathProvider requestPathProvider)
        {
            this.configurationRepository = configurationRepository;
            this.requestRouteProvider = requestRouteProvider;
            this.requestPathProvider = requestPathProvider;
        }

        public PlainHttpRequest Create(HttpRequest httpRequest)
        {
            if (httpRequest == null)
            {
                throw new ArgumentNullException(nameof(httpRequest));
            }

            var configuration = this.configurationRepository.Get();

            var routeConfiguration = this.requestRouteProvider.GetMatchingRouteConfiguration(configuration.Routes, httpRequest);
            if (routeConfiguration == null)
            {
                return null;
            }

            var address = routeConfiguration.Target.Addresses.First();

            var request = new PlainHttpRequest
            {
                Headers = httpRequest.Headers,
                Method = httpRequest.Method,
                Scheme = routeConfiguration.Target.Scheme,
                Host = address.Host,
                Port = address.Port,
                Path = this.requestPathProvider.Get(httpRequest.Path, routeConfiguration.Source.PathTemplate, routeConfiguration.Target.PathTemplate),
                QueryString = httpRequest.QueryString.Value ?? string.Empty,
                // ReSharper disable once PossibleInvalidOperationException
                TimeoutInSeconds = configuration.TimeoutInSeconds.Value
            };

            return request;
        }
    }
}

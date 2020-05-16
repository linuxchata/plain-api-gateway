using System;
using System.Linq;

using Microsoft.AspNetCore.Http;

using PlainApiGateway.Provider.Configuration;
using PlainApiGateway.Provider.Http;
using PlainApiGateway.Repository;

namespace PlainApiGateway.Domain.Http.Factory
{
    public sealed class PlainHttpRequestFactory : IPlainHttpRequestFactory
    {
        private readonly IPlainConfigurationRepository plainConfigurationRepository;

        private readonly IPlainRouteConfigurationProvider plainRouteConfigurationProvider;

        private readonly IHttpRequestPathProvider httpRequestPathProvider;

        public PlainHttpRequestFactory(
            IPlainConfigurationRepository plainConfigurationRepository,
            IPlainRouteConfigurationProvider plainRouteConfigurationProvider,
            IHttpRequestPathProvider httpRequestPathProvider)
        {
            this.plainConfigurationRepository = plainConfigurationRepository;
            this.plainRouteConfigurationProvider = plainRouteConfigurationProvider;
            this.httpRequestPathProvider = httpRequestPathProvider;
        }

        public PlainHttpRequest Create(HttpRequest httpRequest)
        {
            if (httpRequest == null)
            {
                throw new ArgumentNullException(nameof(httpRequest));
            }

            var configuration = this.plainConfigurationRepository.Get();

            var routeConfiguration = this.plainRouteConfigurationProvider.GetMatching(configuration.Routes, httpRequest);
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
                Path = this.httpRequestPathProvider.Get(httpRequest.Path, routeConfiguration.Source.PathTemplate, routeConfiguration.Target.PathTemplate),
                QueryString = httpRequest.QueryString.Value ?? string.Empty,
                // ReSharper disable once PossibleInvalidOperationException
                TimeoutInSeconds = configuration.TimeoutInSeconds.Value
            };

            return request;
        }
    }
}

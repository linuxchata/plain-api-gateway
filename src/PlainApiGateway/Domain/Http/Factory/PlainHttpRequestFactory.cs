using System;
using System.Linq;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using PlainApiGateway.Cache;
using PlainApiGateway.Provider.Configuration;
using PlainApiGateway.Provider.Http;

namespace PlainApiGateway.Domain.Http.Factory
{
    public sealed class PlainHttpRequestFactory : IPlainHttpRequestFactory
    {
        private readonly IPlainConfigurationCache plainConfigurationRepository;

        private readonly IPlainRouteConfigurationProvider plainRouteConfigurationProvider;

        private readonly IHttpRequestPathProvider httpRequestPathProvider;

        private readonly ILogger logger;

        public PlainHttpRequestFactory(
            IPlainConfigurationCache plainConfigurationRepository,
            IPlainRouteConfigurationProvider plainRouteConfigurationProvider,
            IHttpRequestPathProvider httpRequestPathProvider,
            ILoggerFactory logFactory)
        {
            this.plainConfigurationRepository = plainConfigurationRepository;
            this.plainRouteConfigurationProvider = plainRouteConfigurationProvider;
            this.httpRequestPathProvider = httpRequestPathProvider;
            this.logger = logFactory.CreateLogger<PlainHttpRequestFactory>();
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
            string path = this.httpRequestPathProvider.Get(httpRequest.Path, routeConfiguration.Source.PathTemplate, routeConfiguration.Target.PathTemplate);

            var request = new PlainHttpRequest
            {
                Method = httpRequest.Method,
                Scheme = routeConfiguration.Target.Scheme,
                Host = address.Host,
                Port = address.Port,
                Path = path,
                QueryString = httpRequest.QueryString.Value ?? string.Empty,
                Headers = httpRequest.Headers,
                // ReSharper disable once PossibleInvalidOperationException
                TimeoutInSeconds = configuration.TimeoutInSeconds.Value
            };

            this.logger.LogDebug(
                "{method} request has been created for path {path} and query string {queryString}",
                request.Method.ToUpper(),
                request.Path,
                request.QueryString);

            return request;
        }
    }
}

using System;
using System.Linq;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using PlainApiGateway.Configuration;
using PlainApiGateway.Provider.Http;

namespace PlainApiGateway.Domain.Http.Factory
{
    public sealed class PlainHttpRequestFactory : IPlainHttpRequestFactory
    {
        private readonly IHttpRequestPathProvider httpRequestPathProvider;

        private readonly ILogger logger;

        public PlainHttpRequestFactory(
            IHttpRequestPathProvider httpRequestPathProvider,
            ILoggerFactory logFactory)
        {
            this.httpRequestPathProvider = httpRequestPathProvider;
            this.logger = logFactory.CreateLogger<PlainHttpRequestFactory>();
        }

        public PlainHttpRequest Create(HttpRequest httpRequest, PlainRouteConfiguration routeConfiguration, ushort? timeoutInSeconds)
        {
            if (httpRequest == null)
            {
                throw new ArgumentNullException(nameof(httpRequest));
            }

            if (routeConfiguration == null)
            {
                throw new ArgumentNullException(nameof(routeConfiguration));
            }

            var address = routeConfiguration.Target.Addresses.First();

            string path = this.httpRequestPathProvider.Get(
                httpRequest.Path,
                routeConfiguration.Source.PathTemplate,
                routeConfiguration.Target.PathTemplate);

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
                TimeoutInSeconds = timeoutInSeconds.Value
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

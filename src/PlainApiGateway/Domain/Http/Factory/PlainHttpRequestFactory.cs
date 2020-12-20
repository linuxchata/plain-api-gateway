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

        public PlainHttpRequest Create(
            string method,
            string path,
            string queryString,
            IHeaderDictionary headers,
            ushort? timeoutInSeconds,
            PlainRouteConfiguration routeConfiguration)
        {
            this.ValidateParameters(method, headers, routeConfiguration);

            var address = routeConfiguration.Target.Addresses.First();

            var httpPath = this.httpRequestPathProvider.Get(
                path,
                routeConfiguration.Source.PathTemplate,
                routeConfiguration.Target.PathTemplate);

            var request = new PlainHttpRequest
            {
                Method = method,
                Scheme = routeConfiguration.Target.Scheme,
                Host = address.Host,
                Port = address.Port,
                Path = httpPath,
                QueryString = queryString ?? string.Empty,
                Headers = headers,
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

        private void ValidateParameters(string method, IHeaderDictionary headers, PlainRouteConfiguration routeConfiguration)
        {
            if (string.IsNullOrWhiteSpace(method))
            {
                throw new ArgumentNullException(nameof(method));
            }

            if (headers == null)
            {
                throw new ArgumentNullException(nameof(headers));
            }

            if (routeConfiguration == null)
            {
                throw new ArgumentNullException(nameof(routeConfiguration));
            }
        }
    }
}

using System;
using System.Linq;

using Microsoft.AspNetCore.Http;

using PlainApiGateway.Context;
using PlainApiGateway.Repository;

namespace PlainApiGateway.Provider
{
    public sealed class HttpRequestProvider : IHttpRequestProvider
    {
        private readonly IPlainConfigurationRepository configurationRepository;

        public HttpRequestProvider(IPlainConfigurationRepository configurationRepository)
        {
            this.configurationRepository = configurationRepository;
        }

        public RequestContext Create(HttpRequest httpRequest)
        {
            var configuration = this.configurationRepository.Get();

            var route = configuration.Routes
                .FirstOrDefault(a => a.Source.Path == httpRequest.Path);

            if (route == null)
            {
                return null;
            }

            if (!route.Source.HttpMethods.Any(a => string.Equals(a, httpRequest.Method, StringComparison.OrdinalIgnoreCase)))
            {
                return null;
            }

            var address = route.Target.Addresses.FirstOrDefault();
            if (address == null)
            {
                return null;
            }

            var request = new RequestContext
            {
                Headers = httpRequest.Headers,
                Method = httpRequest.Method,
                Scheme = route.Target.Scheme,
                Host = address.Host,
                Port = address.Port,
                Path = route.Target.Path,
                QueryString = httpRequest.QueryString.Value ?? string.Empty
            };

            return request;
        }
    }
}

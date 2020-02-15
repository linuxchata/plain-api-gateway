using System;

using Microsoft.Extensions.DependencyInjection;

using PlainApiGateway.Wrappers;

namespace PlainApiGateway.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPlainApiGateway(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddHttpClient();

            services.AddTransient<IHttpClientWrapper, HttpClientWrapper>();

            return services;
        }
    }
}

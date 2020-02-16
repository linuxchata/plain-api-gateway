using System;
using System.Linq;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using PlainApiGateway.Configuration;
using PlainApiGateway.Handler;
using PlainApiGateway.Provider;
using PlainApiGateway.Repository;
using PlainApiGateway.Wrappers;

namespace PlainApiGateway.Extension
{
    public static class ServiceCollectionExtensions
    {
        private const ushort DefaultTimeoutInSeconds = 30;

        public static IServiceCollection AddPlainApiGateway(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var plainConfiguration = ReadConfiguration(configuration);
            RegisterConfigurationRepository(services, plainConfiguration);

            services.AddHttpClient();

            services.AddLogging();

            services.AddTransient<IErrorHandler, ErrorHandler>();

            services.AddTransient<IRequestRouteProvider, RequestRouteProvider>();
            services.AddTransient<IHttpRequestProvider, HttpRequestProvider>();
            services.AddTransient<IHttpClientWrapper, HttpClientWrapper>();

            return services;
        }

        private static PlainConfiguration ReadConfiguration(IConfiguration configuration)
        {
            var plainConfiguration = new PlainConfiguration();
            configuration.Bind(plainConfiguration);

            if (plainConfiguration.Routes == null || !plainConfiguration.Routes.Any())
            {
                throw new InvalidOperationException("Plain configuration cannot be read");
            }

            plainConfiguration.TimeoutInSeconds = DefaultTimeoutInSeconds;

            return plainConfiguration;
        }

        private static void RegisterConfigurationRepository(IServiceCollection services, PlainConfiguration plainConfiguration)
        {
            var plainConfigurationRepository = new PlainConfigurationRepository();
            plainConfigurationRepository.Add(plainConfiguration);

            services.AddSingleton<IPlainConfigurationRepository>(plainConfigurationRepository);
        }
    }
}

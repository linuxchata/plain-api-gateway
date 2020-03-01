using System;

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

            services.AddHttpClient();

            services.AddLogging();

            var plainConfiguration = ReadConfiguration(configuration);
            RegisterConfigurationRepository(services, plainConfiguration);

            services.AddTransient<IErrorHandler, ErrorHandler>();

            services.AddTransient<IRequestRouteProvider, RequestRouteProvider>();
            services.AddTransient<IRequestPathProvider, RequestPathProvider>();
            services.AddTransient<IHttpRequestProvider, HttpRequestProvider>();
            services.AddTransient<IHttpClientWrapper, HttpClientWrapper>();

            return services;
        }

        private static PlainConfiguration ReadConfiguration(IConfiguration configuration)
        {
            var plainConfigurationReader = new PlainConfigurationReader();
            return plainConfigurationReader.Read(configuration);
        }

        private static void RegisterConfigurationRepository(IServiceCollection services, PlainConfiguration plainConfiguration)
        {
            var plainConfigurationRepository = new PlainConfigurationRepository();
            plainConfigurationRepository.Add(plainConfiguration);

            services.AddSingleton<IPlainConfigurationRepository>(plainConfigurationRepository);
        }
    }
}

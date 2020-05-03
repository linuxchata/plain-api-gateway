using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using PlainApiGateway.Domain.Entity.Configuration;
using PlainApiGateway.Handler;
using PlainApiGateway.Provider;
using PlainApiGateway.Provider.Configuration;
using PlainApiGateway.Repository;
using PlainApiGateway.Wrapper;

namespace PlainApiGateway.Extension
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add Plain API gateway to collection of services
        /// </summary>
        /// <param name="services">Services collection</param>
        /// <param name="configuration">The configuration</param>
        /// <returns>Returns services collection</returns>
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
            var plainConfigurationProvider = new PlainConfigurationProvider();
            return plainConfigurationProvider.Read(configuration);
        }

        private static void RegisterConfigurationRepository(IServiceCollection services, PlainConfiguration plainConfiguration)
        {
            var plainConfigurationRepository = new PlainConfigurationRepository();
            plainConfigurationRepository.Add(plainConfiguration);

            services.AddSingleton<IPlainConfigurationRepository>(plainConfigurationRepository);
        }
    }
}

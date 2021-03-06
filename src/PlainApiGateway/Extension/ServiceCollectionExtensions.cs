﻿using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using PlainApiGateway.Cache;
using PlainApiGateway.Configuration;
using PlainApiGateway.Domain.Http.Factory;
using PlainApiGateway.Handler;
using PlainApiGateway.Provider.Configuration;
using PlainApiGateway.Provider.Http;
using PlainApiGateway.Wrapper;

namespace PlainApiGateway.Extension
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Plain API gateway to collection of services
        /// </summary>
        /// <param name="services">Services collection</param>
        /// <param name="configuration">The configuration</param>
        /// <returns>Services collection</returns>
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
            RegisterConfigurationCache(services, plainConfiguration);

            services.AddTransient<IErrorHandler, ErrorHandler>();

            services.AddTransient<IPlainRouteConfigurationProvider, PlainRouteConfigurationProvider>();
            services.AddTransient<IHttpRequestPathProvider, HttpRequestPathProvider>();
            services.AddTransient<IPlainHttpRequestFactory, PlainHttpRequestFactory>();
            services.AddTransient<IHttpClientWrapper, HttpClientWrapper>();

            return services;
        }

        private static PlainConfiguration ReadConfiguration(IConfiguration configuration)
        {
            var plainConfigurationProvider = new PlainConfigurationProvider();
            return plainConfigurationProvider.Read(configuration);
        }

        private static void RegisterConfigurationCache(IServiceCollection services, PlainConfiguration plainConfiguration)
        {
            var plainConfigurationCache = new PlainConfigurationCache();
            plainConfigurationCache.Add(plainConfiguration);

            services.AddSingleton<IPlainConfigurationCache>(plainConfigurationCache);
        }
    }
}

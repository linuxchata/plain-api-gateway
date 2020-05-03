using System;
using System.Linq;

using Microsoft.Extensions.Configuration;

using PlainApiGateway.Domain.Entity.Configuration;

namespace PlainApiGateway.Provider.Configuration
{
    public sealed class PlainConfigurationProvider : IPlainConfigurationProvider
    {
        private const ushort DefaultTimeoutInSeconds = 30;

        public PlainConfiguration Read(IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var plainConfiguration = new PlainConfiguration();
            configuration.Bind(plainConfiguration);

            if (plainConfiguration.Routes == null || !plainConfiguration.Routes.Any())
            {
                throw new InvalidOperationException("Plain configuration routes cannot be read");
            }

            if (!plainConfiguration.TimeoutInSeconds.HasValue)
            {
                plainConfiguration.TimeoutInSeconds = DefaultTimeoutInSeconds;
            }

            return plainConfiguration;
        }
    }
}

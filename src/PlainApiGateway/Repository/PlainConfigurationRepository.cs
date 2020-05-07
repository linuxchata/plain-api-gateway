using System;

using PlainApiGateway.Domain.Entity.Configuration;

namespace PlainApiGateway.Repository
{
    public sealed class PlainConfigurationRepository : IPlainConfigurationRepository
    {
        private PlainConfiguration plainConfigurationCache;

        public PlainConfiguration Get()
        {
            if (this.plainConfigurationCache == null)
            {
                throw new ArgumentNullException(nameof(this.plainConfigurationCache));
            }

            return this.plainConfigurationCache;
        }

        public void Add(PlainConfiguration plainConfiguration)
        {
            if (plainConfiguration == null)
            {
                throw new ArgumentNullException(nameof(plainConfiguration));
            }

            this.plainConfigurationCache = plainConfiguration;
        }
    }
}

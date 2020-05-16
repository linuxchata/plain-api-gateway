using System;

using PlainApiGateway.Configuration;

namespace PlainApiGateway.Cache
{
    public sealed class PlainConfigurationCache : IPlainConfigurationCache
    {
        private PlainConfiguration cache;

        public PlainConfiguration Get()
        {
            if (this.cache == null)
            {
                throw new ArgumentNullException(nameof(this.cache));
            }

            return this.cache;
        }

        public void Add(PlainConfiguration plainConfiguration)
        {
            if (plainConfiguration == null)
            {
                throw new ArgumentNullException(nameof(plainConfiguration));
            }

            this.cache = plainConfiguration;
        }
    }
}

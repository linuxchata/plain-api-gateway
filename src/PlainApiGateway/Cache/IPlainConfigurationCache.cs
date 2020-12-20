using PlainApiGateway.Configuration;

namespace PlainApiGateway.Cache
{
    public interface IPlainConfigurationCache
    {
        /// <summary>
        /// Gets configuration from the cache
        /// </summary>
        /// <returns>The configuration</returns>
        PlainConfiguration Get();

        /// <summary>
        /// Adds configuration to the cache
        /// </summary>
        /// <param name="plainConfiguration">The configuration</param>
        void Add(PlainConfiguration plainConfiguration);
    }
}
using PlainApiGateway.Configuration;

namespace PlainApiGateway.Cache
{
    public interface IPlainConfigurationCache
    {
        /// <summary>
        /// Gets configuration
        /// </summary>
        /// <returns>Return configuration</returns>
        PlainConfiguration Get();

        /// <summary>
        /// Adds configuration
        /// </summary>
        /// <param name="plainConfiguration">The configuration</param>
        void Add(PlainConfiguration plainConfiguration);
    }
}
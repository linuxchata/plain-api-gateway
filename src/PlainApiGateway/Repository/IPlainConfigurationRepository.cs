using PlainApiGateway.Configuration;

namespace PlainApiGateway.Repository
{
    public interface IPlainConfigurationRepository
    {
        /// <summary>
        /// Gets configuration
        /// </summary>
        /// <returns>Return configuration</returns>
        PlainConfiguration Get();

        /// <summary>
        /// Add configuration
        /// </summary>
        /// <param name="plainConfiguration">The configuration</param>
        void Add(PlainConfiguration plainConfiguration);
    }
}
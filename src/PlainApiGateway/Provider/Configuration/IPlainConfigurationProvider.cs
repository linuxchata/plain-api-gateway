using Microsoft.Extensions.Configuration;

using PlainApiGateway.Configuration;

namespace PlainApiGateway.Provider.Configuration
{
    public interface IPlainConfigurationProvider
    {
        /// <summary>
        /// Reads plain configuration
        /// </summary>
        /// <param name="configuration">The configuration</param>
        /// <returns>The plain configuration</returns>
        PlainConfiguration Read(IConfiguration configuration);
    }
}
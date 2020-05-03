using Microsoft.Extensions.Configuration;

using PlainApiGateway.Domain.Entity.Configuration;

namespace PlainApiGateway.Configuration
{
    public interface IPlainConfigurationReader
    {
        /// <summary>
        /// Reads configuration
        /// </summary>
        /// <param name="configuration">The configuration</param>
        /// <returns>Returns configuration</returns>
        PlainConfiguration Read(IConfiguration configuration);
    }
}
using Microsoft.Extensions.Configuration;

namespace PlainApiGateway.Configuration
{
    public interface IPlainConfigurationReader
    {
        PlainConfiguration Read(IConfiguration configuration);
    }
}
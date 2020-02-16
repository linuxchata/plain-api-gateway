using PlainApiGateway.Configuration;

namespace PlainApiGateway.Repository
{
    public interface IPlainConfigurationRepository
    {
        PlainConfiguration Get();

        void Add(PlainConfiguration plainConfiguration);
    }
}
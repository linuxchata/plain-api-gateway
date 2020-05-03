namespace PlainApiGateway.Domain.Entity.Configuration
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class PlainRouteConfiguration
    {
        public PlainRouteSourceConfiguration Source { get; set; }

        public PlainRouteTargetConfiguration Target { get; set; }
    }
}

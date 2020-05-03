namespace PlainApiGateway.Configuration
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class PlainRouteTargetConfiguration
    {
        public string Scheme { get; set; }

        public PlainRouteTargetAddressConfiguration[] Addresses { get; set; }

        public string PathTemplate { get; set; }
    }
}

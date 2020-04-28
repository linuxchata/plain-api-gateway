namespace PlainApiGateway.Configuration
{
    public sealed class PlainRouteTargetConfiguration
    {
        public string Scheme { get; set; }

        public PlainRouteTargetAddressConfiguration[] Addresses { get; set; }

        public string PathTemplate { get; set; }
    }
}

namespace PlainApiGateway.Configuration
{
    public sealed class PlainRouteSourceConfiguration
    {
        public string PathTemplate { get; set; }

        public string[] HttpMethods { get; set; }
    }
}

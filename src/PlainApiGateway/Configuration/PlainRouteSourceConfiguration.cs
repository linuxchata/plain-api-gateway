namespace PlainApiGateway.Configuration
{
    public sealed class PlainRouteSourceConfiguration
    {
        public string Path { get; set; }

        public string[] HttpMethods { get; set; }
    }
}

namespace PlainApiGateway.Configuration
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class PlainRouteSourceConfiguration
    {
        public string PathTemplate { get; set; }

        public string[] HttpMethods { get; set; }
    }
}

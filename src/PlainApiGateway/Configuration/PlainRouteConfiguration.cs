namespace PlainApiGateway.Configuration
{
    public sealed class PlainRouteConfiguration
    {
        public string SourcePath { get; set; }

        public string[] SourceHttpMethods { get; set; }

        public string TargetScheme { get; set; }

        public string TargetHost { get; set; }

        public string TargetPort { get; set; }

        public string TargetPath { get; set; }
    }
}

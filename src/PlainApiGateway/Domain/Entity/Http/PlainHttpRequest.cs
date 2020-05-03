using Microsoft.AspNetCore.Http;

namespace PlainApiGateway.Domain.Entity.Http
{
    public sealed class PlainHttpRequest
    {
        public IHeaderDictionary Headers { get; set; }

        public string Method { get; set; }

        public string Scheme { get; set; }

        public string Host { get; set; }

        public ushort Port { get; set; }

        public string Path { get; set; }

        public string QueryString { get; set; }

        public ushort TimeoutInSeconds { get; set; }
    }
}
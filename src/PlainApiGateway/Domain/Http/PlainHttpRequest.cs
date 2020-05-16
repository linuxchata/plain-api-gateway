using System;

using Microsoft.AspNetCore.Http;

namespace PlainApiGateway.Domain.Http
{
    public sealed class PlainHttpRequest
    {
        public Guid Id { get; set; }

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
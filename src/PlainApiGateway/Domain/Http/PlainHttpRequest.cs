using System;

using Microsoft.AspNetCore.Http;

namespace PlainApiGateway.Domain.Http
{
    public sealed class PlainHttpRequest : Entity
    {
        public PlainHttpRequest()
        {
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; }

        public string Method { get; set; }

        public string Scheme { get; set; }

        public string Host { get; set; }

        public ushort Port { get; set; }

        public string Path { get; set; }

        public string QueryString { get; set; }

        public IHeaderDictionary Headers { get; set; }

        public ushort TimeoutInSeconds { get; set; }
    }
}
using System;

using Microsoft.AspNetCore.Http;

namespace PlainApiGateway.Context
{
    public sealed class RequestContext
    {
        public IHeaderDictionary Headers { get; set; }

        public string Method { get; set; }

        public string Scheme { get; set; }

        public string Host { get; set; }

        public ushort Port { get; set; }

        public string Path { get; set; }

        public string QueryString { get; set; }

        public ushort TimeoutInSeconds { get; set; }

        public string GetUrl()
        {
            var urlBuilder = new UriBuilder
            {
                Scheme = this.Scheme,
                Host = this.Host,
                Port = this.Port,
                Path = this.Path,
                Query = this.QueryString
            };

            return urlBuilder.Uri.ToString();
        }
    }
}
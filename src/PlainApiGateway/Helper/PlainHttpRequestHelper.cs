using System;

using PlainApiGateway.Domain.Entity;

namespace PlainApiGateway.Helper
{
    public static class PlainHttpRequestHelper
    {
        public static string GetUrl(PlainHttpRequest plainHttpRequest)
        {
            if (plainHttpRequest == null)
            {
                throw new ArgumentNullException(nameof(plainHttpRequest));
            }

            var urlBuilder = new UriBuilder
            {
                Scheme = plainHttpRequest.Scheme,
                Host = plainHttpRequest.Host,
                Port = plainHttpRequest.Port,
                Path = plainHttpRequest.Path,
                Query = plainHttpRequest.QueryString
            };

            return urlBuilder.Uri.ToString();
        }
    }
}
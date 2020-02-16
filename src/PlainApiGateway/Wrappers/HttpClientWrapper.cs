using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PlainApiGateway.Wrappers
{
    public sealed class HttpClientWrapper : IHttpClientWrapper
    {
        private readonly IHttpClientFactory httpClientFactory;

        public HttpClientWrapper(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<HttpResponseMessage> SendRequest(string httpMethod, string requestUrl, int timeoutInSeconds)
        {
            if (string.IsNullOrWhiteSpace(httpMethod))
            {
                throw new ArgumentNullException(nameof(httpMethod));
            }

            if (string.IsNullOrWhiteSpace(requestUrl))
            {
                throw new ArgumentNullException(nameof(requestUrl));
            }

            if (timeoutInSeconds <= 0)
            {
                throw new ArgumentException(nameof(timeoutInSeconds));
            }

            var client = this.httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(timeoutInSeconds);

            var httpMethodValue = new HttpMethod(httpMethod);
            var response = await client.SendAsync(new HttpRequestMessage(httpMethodValue, requestUrl));

            return response;
        }
    }
}

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

        public async Task<HttpResponseMessage> SendRequest(string httpMethod, string requestUrl)
        {
            var client = this.httpClientFactory.CreateClient();

            var httpMethodValue = new HttpMethod(httpMethod);
            var response = await client.SendAsync(new HttpRequestMessage(httpMethodValue, requestUrl));

            return response;
        }
    }
}

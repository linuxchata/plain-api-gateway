using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace PlainApiGateway.Wrapper
{
    public sealed class HttpClientWrapper : IHttpClientWrapper
    {
        private readonly IHttpClientFactory httpClientFactory;

        private readonly ILogger logger;

        public HttpClientWrapper(IHttpClientFactory httpClientFactory, ILoggerFactory logFactory)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logFactory.CreateLogger<HttpClientWrapper>();
        }

        public async Task<HttpResponseMessage> SendRequest(
            string requestUrl,
            string httpMethod,
            Stream requestBodyStream,
            IHeaderDictionary headers,
            int timeoutInSeconds)
        {
            ValidateParameters(requestUrl, httpMethod, headers);

            var client = this.CreateHttpClient(timeoutInSeconds);

            var requestMessage = this.CreateHttpRequestMessage(requestUrl, httpMethod, requestBodyStream, headers);

            var response = await client.SendAsync(requestMessage);

            this.LogResult(requestUrl, httpMethod, response.StatusCode);

            return response;
        }

        private static void ValidateParameters(string requestUrl, string httpMethod, IHeaderDictionary headers)
        {
            if (string.IsNullOrWhiteSpace(requestUrl))
            {
                throw new ArgumentNullException(nameof(requestUrl));
            }

            if (string.IsNullOrWhiteSpace(httpMethod))
            {
                throw new ArgumentNullException(nameof(httpMethod));
            }

            if (headers == null)
            {
                throw new ArgumentNullException(nameof(headers));
            }
        }

        private HttpClient CreateHttpClient(int timeoutInSeconds)
        {
            var client = this.httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(timeoutInSeconds);
            return client;
        }

        private HttpRequestMessage CreateHttpRequestMessage(
            string requestUrl,
            string httpMethod,
            Stream requestStream,
            IHeaderDictionary headers)
        {
            var requestMessage = new HttpRequestMessage(new HttpMethod(httpMethod), requestUrl);

            this.SetContentIfAny(requestStream, requestMessage);

            this.SetHeaders(headers, requestMessage);

            return requestMessage;
        }

        private void SetContentIfAny(Stream requestStream, HttpRequestMessage requestMessage)
        {
            if (requestStream != null && requestStream.CanRead)
            {
                requestMessage.Content = new StreamContent(requestStream);
            }
        }

        private void SetHeaders(IHeaderDictionary headers, HttpRequestMessage requestMessage)
        {
            foreach (var header in headers)
            {
                if (string.Equals(header.Key, HeaderNames.ContentType, StringComparison.OrdinalIgnoreCase))
                {
                    requestMessage.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue(header.Value);
                }
                else if (string.Equals(header.Key, HeaderNames.ContentLength, StringComparison.OrdinalIgnoreCase))
                {
                    long contentLengthValue = Convert.ToInt64(header.Value.ToString());
                    requestMessage.Content.Headers.ContentLength = contentLengthValue;
                }
                else
                {
                    requestMessage.Headers.Add(header.Key, new string[] { header.Value });
                }
            }
        }

        private void LogResult(string requestUrl, string httpMethod, HttpStatusCode httpStatusCode)
        {
            this.logger.LogDebug(
                "{httpMethod} request to URL {requestUrl} has been completed with status code {httpStatusCode}",
                httpMethod,
                requestUrl,
                httpStatusCode);
        }
    }
}

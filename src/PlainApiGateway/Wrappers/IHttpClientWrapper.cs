using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace PlainApiGateway.Wrappers
{
    public interface IHttpClientWrapper
    {
        /// <summary>
        /// Sends HTTP request
        /// </summary>
        /// <param name="requestUrl">Request URL</param>
        /// <param name="httpMethod">HTTP method</param>
        /// <param name="requestBodyStream">Request body</param>
        /// <param name="headers">HTTP headers</param>
        /// <param name="timeoutInSeconds">Timeout in seconds</param>
        /// <returns>Returns HTTP response message</returns>
        Task<HttpResponseMessage> SendRequest(
            string requestUrl,
            string httpMethod,
            Stream requestBodyStream,
            IHeaderDictionary headers,
            int timeoutInSeconds);
    }
}

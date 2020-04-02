using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace PlainApiGateway.Wrappers
{
    public interface IHttpClientWrapper
    {
        Task<HttpResponseMessage> SendRequest(
            string requestUrl,
            string httpMethod,
            Stream requestBodyStream,
            IHeaderDictionary headers,
            int timeoutInSeconds);
    }
}

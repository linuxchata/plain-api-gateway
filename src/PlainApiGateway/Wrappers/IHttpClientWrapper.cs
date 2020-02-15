using System.Net.Http;
using System.Threading.Tasks;

namespace PlainApiGateway.Wrappers
{
    public interface IHttpClientWrapper
    {
        Task<HttpResponseMessage> SendRequest(string httpMethod, string requestUrl);
    }
}

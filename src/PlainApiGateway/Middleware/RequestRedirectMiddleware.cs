using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

using PlainApiGateway.Extensions;
using PlainApiGateway.Wrappers;

namespace PlainApiGateway.Middleware
{
    public sealed class RequestRedirectMiddleware
    {
        private readonly RequestDelegate next;

        private readonly IHttpClientWrapper httpClientWrapper;

        public RequestRedirectMiddleware(
            RequestDelegate next,
            IHttpClientWrapper httpClientWrapper)
        {
            this.next = next;
            this.httpClientWrapper = httpClientWrapper;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string host = context.Request.Scheme + "://" + context.Request.Host.Host + ":9002";
            var response = await this.httpClientWrapper.SendRequest(context.Request.Method, host);

            var plainContext = context.GetPlainContext();
            plainContext.Response = response;

            await this.next(context);
        }
    }
}

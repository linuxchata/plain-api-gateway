using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

using PlainApiGateway.Extension;
using PlainApiGateway.Provider;
using PlainApiGateway.Wrappers;

namespace PlainApiGateway.Middleware
{
    public sealed class RequestRedirectMiddleware
    {
        private readonly RequestDelegate next;

        private readonly IHttpRequestProvider httpRequestProvider;

        private readonly IHttpClientWrapper httpClientWrapper;

        public RequestRedirectMiddleware(
            RequestDelegate next,
            IHttpRequestProvider httpRequestProvider,
            IHttpClientWrapper httpClientWrapper)
        {
            this.next = next;
            this.httpRequestProvider = httpRequestProvider;
            this.httpClientWrapper = httpClientWrapper;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var request = this.httpRequestProvider.Create(context.Request);
            var response = await this.httpClientWrapper.SendRequest(context.Request.Method, request.GetUrl());

            var plainContext = context.GetPlainContext();
            plainContext.Response = response;

            //var con = await plainContext.Response.Content.ReadAsStreamAsync();

            await this.next(context);
        }
    }
}

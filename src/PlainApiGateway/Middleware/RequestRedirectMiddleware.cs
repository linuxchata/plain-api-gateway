using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

using PlainApiGateway.Domain.Entity.Http;
using PlainApiGateway.Extension;
using PlainApiGateway.Handler;
using PlainApiGateway.Helper;
using PlainApiGateway.Provider;
using PlainApiGateway.Wrapper;

namespace PlainApiGateway.Middleware
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class RequestRedirectMiddleware
    {
        private readonly RequestDelegate next;

        private readonly IHttpRequestProvider httpRequestProvider;

        private readonly IHttpClientWrapper httpClientWrapper;

        private HttpContext httpContext;

        public RequestRedirectMiddleware(
            RequestDelegate next,
            IHttpRequestProvider httpRequestProvider,
            IHttpClientWrapper httpClientWrapper)
        {
            this.next = next;
            this.httpRequestProvider = httpRequestProvider;
            this.httpClientWrapper = httpClientWrapper;
        }

        public async Task InvokeAsync(HttpContext context, IErrorHandler errorHandler)
        {
            this.httpContext = context;

            var plainHttpRequest = this.CreatePlainHttpRequest();
            if (this.IsRequestValid(plainHttpRequest))
            {
                SetRouteNotFoundErrorResponse(errorHandler);
                return;
            }

            var response = await this.SendHttpRequest(plainHttpRequest);

            this.AssignResponseToHttpContext(response);

            await this.next(this.httpContext);
        }

        private PlainHttpRequest CreatePlainHttpRequest()
        {
            return this.httpRequestProvider.Create(this.httpContext.Request);
        }

        private bool IsRequestValid(PlainHttpRequest plainHttpRequest)
        {
            return plainHttpRequest == null;
        }

        private void SetRouteNotFoundErrorResponse(IErrorHandler errorHandler)
        {
            errorHandler.SetRouteNotFoundErrorResponse(this.httpContext);
        }

        private async Task<HttpResponseMessage> SendHttpRequest(PlainHttpRequest plainHttpRequest)
        {
            string requestUrl = PlainHttpRequestHelper.GetUrl(plainHttpRequest);

            var response = await this.httpClientWrapper.SendRequest(
                requestUrl,
                this.httpContext.Request.Method,
                this.httpContext.Request.Body,
                plainHttpRequest.Headers,
                plainHttpRequest.TimeoutInSeconds);

            return response;
        }

        private void AssignResponseToHttpContext(HttpResponseMessage response)
        {
            var plainHttpContext = this.httpContext.CreatePlainHttpContext();
            plainHttpContext.Response = response;
        }
    }
}

using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using PlainApiGateway.Cache;
using PlainApiGateway.Domain.Http;
using PlainApiGateway.Domain.Http.Factory;
using PlainApiGateway.Extension;
using PlainApiGateway.Handler;
using PlainApiGateway.Helper;
using PlainApiGateway.Provider.Configuration;
using PlainApiGateway.Wrapper;

namespace PlainApiGateway.Middleware
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class RequestRedirectMiddleware
    {
        private readonly RequestDelegate next;

        private readonly IPlainConfigurationCache plainConfigurationCache;

        private readonly IPlainRouteConfigurationProvider plainRouteConfigurationProvider;

        private readonly IPlainHttpRequestFactory plainHttpRequestFactory;

        private readonly IHttpClientWrapper httpClientWrapper;

        private readonly ILogger logger;

        private HttpContext httpContext;

        public RequestRedirectMiddleware(
            RequestDelegate next,
            IPlainConfigurationCache plainConfigurationCache,
            IPlainRouteConfigurationProvider plainRouteConfigurationProvider,
            IPlainHttpRequestFactory plainHttpRequestFactory,
            IHttpClientWrapper httpClientWrapper,
            ILoggerFactory logFactory)
        {
            this.next = next;
            this.plainConfigurationCache = plainConfigurationCache;
            this.plainRouteConfigurationProvider = plainRouteConfigurationProvider;
            this.plainHttpRequestFactory = plainHttpRequestFactory;
            this.httpClientWrapper = httpClientWrapper;
            this.logger = logFactory.CreateLogger<RequestRedirectMiddleware>();
        }

        public async Task InvokeAsync(HttpContext context, IErrorHandler errorHandler)
        {
            this.httpContext = context;

            var plainHttpRequest = this.CreatePlainHttpRequest();
            if (this.IsRequestValid(plainHttpRequest))
            {
                this.SetRouteNotFoundErrorResponse(errorHandler);
                return;
            }

            var response = await this.SendHttpRequest(plainHttpRequest);

            this.AssignResponseToHttpContext(response);

            await this.next(this.httpContext);
        }

        private PlainHttpRequest CreatePlainHttpRequest()
        {
            var configuration = this.plainConfigurationCache.Get();

            var routeConfiguration = this.plainRouteConfigurationProvider.GetMatching(configuration?.Routes, this.httpContext.Request);
            if (routeConfiguration == null)
            {
                return null;
            }

            var httpRequest = this.httpContext.Request;

            var request = this.plainHttpRequestFactory.Create(
                httpRequest.Method,
                httpRequest.Path,
                httpRequest.QueryString.Value,
                httpRequest.Headers,
                configuration!.TimeoutInSeconds,
                routeConfiguration);

            return request;
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
            var requestUrl = PlainHttpRequestHelper.GetUrl(plainHttpRequest);

            var response = await this.httpClientWrapper.SendRequest(
                requestUrl,
                this.httpContext.Request.Method,
                this.httpContext.Request.Body,
                plainHttpRequest.Headers,
                plainHttpRequest.TimeoutInSeconds);

            this.logger.LogDebug(
                "{method} request has been send to URL {requestUrl}. Response status code {statusCode}",
                this.httpContext.Request.Method.ToUpper(),
                requestUrl,
                response.StatusCode);

            return response;
        }

        private void AssignResponseToHttpContext(HttpResponseMessage response)
        {
            var plainHttpContext = this.httpContext.CreatePlainHttpContext();
            plainHttpContext.SetResponse(response);
        }
    }
}

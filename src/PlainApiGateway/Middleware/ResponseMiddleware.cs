using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

using PlainApiGateway.Domain.Http;
using PlainApiGateway.Extension;

namespace PlainApiGateway.Middleware
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class ResponseMiddleware
    {
        private readonly RequestDelegate next;

        private readonly ILogger logger;

        private HttpContext httpContext;

        private PlainHttpContext plainHttpContext;

        public ResponseMiddleware(RequestDelegate next, ILoggerFactory logFactory)
        {
            this.next = next;
            this.logger = logFactory.CreateLogger<ResponseMiddleware>();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            this.httpContext = context;

            await this.next(this.httpContext);

            this.plainHttpContext = this.httpContext.GetPlainHttpContext();

            this.SetStatusCode();

            this.SetHeaders();

            await this.CopyContent();

            this.LogResult();
        }

        private void SetStatusCode()
        {
            if (!this.httpContext.Response.HasStarted)
            {
                this.httpContext.Response.StatusCode = (int)this.plainHttpContext.Response.StatusCode;
            }
        }

        private void SetHeaders()
        {
            var contentLengthHeader = this.CreateContentLengthHeader();
            this.SetHeader(this.httpContext.Response, contentLengthHeader);

            foreach (var header in this.plainHttpContext.Response.Content.Headers)
            {
                if (!this.httpContext.Response.Headers.ContainsKey(header.Key))
                {
                    this.httpContext.Response.Headers.Add(header.Key, new StringValues(header.Value.ToArray()));
                }
            }
        }

        private PlainHttpHeader CreateContentLengthHeader()
        {
            string[] contentLengthValue = { this.plainHttpContext.Response.Content.Headers.ContentLength.ToString() };
            return new PlainHttpHeader(HeaderNames.ContentLength, contentLengthValue);
        }

        private void SetHeader(HttpResponse response, PlainHttpHeader header)
        {
            if (!response.Headers.ContainsKey(header.Key))
            {
                response.Headers.Add(header.Key, header.Value);
            }
        }

        private async Task CopyContent()
        {
            if (this.plainHttpContext.Response.Content == null)
            {
                return;
            }

            using (var contentStream = await this.plainHttpContext.Response.Content.ReadAsStreamAsync())
            {
                await contentStream.CopyToAsync(this.httpContext.Response.Body);
            }
        }

        private void LogResult()
        {
            this.logger.LogDebug(
                "Response for {method} request to URL {url} has been mapped",
                this.httpContext.Request.Method.ToUpper(),
                this.httpContext.Request.GetDisplayUrl());
        }
    }
}

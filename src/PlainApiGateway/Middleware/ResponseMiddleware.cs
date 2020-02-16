using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

using PlainApiGateway.Context;
using PlainApiGateway.Extension;
using PlainApiGateway.Model;

namespace PlainApiGateway.Middleware
{
    public sealed class ResponseMiddleware
    {
        private readonly RequestDelegate next;

        public ResponseMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await this.next(context);

            var plainContext = context.GetPlainContext();

            SetStatusCode(context, plainContext);

            var contentLengthHeader = CreateContentLengthHeader(plainContext);
            SetHeader(context.Response, contentLengthHeader);

            SetHeaders(context.Response, plainContext);

            await CopyContent(context, plainContext);
        }

        private static void SetStatusCode(HttpContext context, PlainContext plainContext)
        {
            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = (int)plainContext.Response.StatusCode;
            }
        }

        private static Header CreateContentLengthHeader(PlainContext plainContext)
        {
            string[] contentLengthValue = { plainContext.Response.Content.Headers.ContentLength.ToString() };
            return new Header(HeaderNames.ContentLength, contentLengthValue);
        }

        private static void SetHeader(HttpResponse response, Header header)
        {
            if (!response.Headers.ContainsKey(header.Key))
            {
                response.Headers.Add(header.Key, header.Value);
            }
        }

        private static void SetHeaders(HttpResponse response, PlainContext plainContext)
        {
            foreach (var header in plainContext.Response.Content.Headers)
            {
                if (!response.Headers.ContainsKey(header.Key))
                {
                    response.Headers.Add(header.Key, new StringValues(header.Value.ToArray()));
                }
            }
        }

        private static async Task CopyContent(HttpContext context, PlainContext plainContext)
        {
            if (plainContext.Response.Content == null)
            {
                return;
            }

            using (var contentStream = await plainContext.Response.Content.ReadAsStreamAsync())
            {
                await contentStream.CopyToAsync(context.Response.Body);
            }
        }
    }
}

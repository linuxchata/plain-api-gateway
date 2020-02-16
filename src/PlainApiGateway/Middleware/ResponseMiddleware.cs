using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

using PlainApiGateway.Context;
using PlainApiGateway.Extension;

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

            SetHeaders(context, plainContext.Response.Headers);

            if (!context.Response.HasStarted)
            {
                SetStatusCode(context, plainContext);
            }
        }

        private static void SetHeaders(HttpContext context, HttpResponseHeaders httpResponseHeaders)
        {
            foreach (var httpResponseHeader in httpResponseHeaders)
            {
                if (!context.Response.Headers.ContainsKey(httpResponseHeader.Key))
                {
                    context.Response.Headers.Add(httpResponseHeader.Key, new StringValues(httpResponseHeader.Value.ToArray()));
                }
            }
        }

        private static void SetStatusCode(HttpContext context, PlainContext plainContext)
        {
            context.Response.StatusCode = (int)plainContext.Response.StatusCode;
        }
    }
}

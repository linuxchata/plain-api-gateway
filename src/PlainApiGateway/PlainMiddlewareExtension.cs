using System;

using Microsoft.AspNetCore.Builder;

using PlainApiGateway.Middleware;

namespace PlainApiGateway
{
    public static class PlainMiddlewareExtension
    {
        public static IApplicationBuilder UsePlainApiGateway(this IApplicationBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.UseMiddleware<RequestRedirectMiddleware>();
            builder.UseMiddleware<ResponseMiddleware>();

            return builder;
        }
    }
}

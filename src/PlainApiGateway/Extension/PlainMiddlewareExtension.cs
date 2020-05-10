using System;

using Microsoft.AspNetCore.Builder;

using PlainApiGateway.Middleware;

namespace PlainApiGateway.Extension
{
    public static class PlainMiddlewareExtension
    {
        public static IApplicationBuilder UsePlainApiGateway(this IApplicationBuilder builder, PlainMiddlewareConfiguration plainMiddlewareConfiguration = null)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            AddPreRequestMiddleware(builder, plainMiddlewareConfiguration);

            AddRequestRedirectMiddleware(builder);

            AddPreResponseMiddleware(builder, plainMiddlewareConfiguration);

            AddResponseMiddleware(builder);

            return builder;
        }

        private static void AddPreRequestMiddleware(IApplicationBuilder builder, PlainMiddlewareConfiguration plainMiddlewareConfiguration)
        {
            if (plainMiddlewareConfiguration?.PreRequestMiddleware != null)
            {
                builder.Use(plainMiddlewareConfiguration.PreRequestMiddleware);
            }
        }

        private static void AddRequestRedirectMiddleware(IApplicationBuilder builder)
        {
            builder.UseMiddleware<RequestRedirectMiddleware>();
        }

        private static void AddPreResponseMiddleware(IApplicationBuilder builder, PlainMiddlewareConfiguration plainMiddlewareConfiguration)
        {
            if (plainMiddlewareConfiguration?.PreResponseMiddleware != null)
            {
                builder.Use(plainMiddlewareConfiguration.PreResponseMiddleware);
            }
        }

        private static void AddResponseMiddleware(IApplicationBuilder builder)
        {
            builder.UseMiddleware<ResponseMiddleware>();
        }
    }
}

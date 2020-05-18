using System;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;

namespace PlainApiGateway.Handler
{
    public sealed class ErrorHandler : IErrorHandler
    {
        private readonly ILogger logger;

        public ErrorHandler(ILoggerFactory logger)
        {
            this.logger = logger.CreateLogger<ErrorHandler>();
        }

        public void SetRouteNotFoundErrorResponse(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (!context.Response.HasStarted)
            {
                this.logger.LogWarning(
                    "Corresponding route for {method} request to URL {url} has not been found",
                    context.Request.Method.ToUpper(),
                    context.Request.GetDisplayUrl());

                context.Response.StatusCode = StatusCodes.Status404NotFound;
            }
        }
    }
}
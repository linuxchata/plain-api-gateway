using Microsoft.AspNetCore.Http;
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
            if (!context.Response.HasStarted)
            {
                this.logger.LogWarning(
                    "Corresponding route for {method} request to [{path}] path has not been found",
                    context.Request.Method,
                    context.Request.Path);

                context.Response.StatusCode = StatusCodes.Status404NotFound;
            }
        }
    }
}
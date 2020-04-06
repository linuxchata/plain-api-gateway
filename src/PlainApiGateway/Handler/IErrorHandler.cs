using Microsoft.AspNetCore.Http;

namespace PlainApiGateway.Handler
{
    public interface IErrorHandler
    {
        /// <summary>
        /// Sets route not found error response
        /// </summary>
        /// <param name="context">HTTP context</param>
        void SetRouteNotFoundErrorResponse(HttpContext context);
    }
}
using Microsoft.AspNetCore.Http;

namespace PlainApiGateway.Handler
{
    public interface IErrorHandler
    {
        void SetRouteNotFoundErrorResponse(HttpContext context);
    }
}
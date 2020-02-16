using System;

using Microsoft.AspNetCore.Http;

using PlainApiGateway.Context;

namespace PlainApiGateway.Extension
{
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Extension method for getting the <see cref="PlainContext"/> for the current request.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/> context.</param>
        /// <returns>The <see cref="PlainContext"/>.</returns>
        public static PlainContext GetPlainContext(this HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var plainContext = context.Features.Get<PlainContext>();

            if (plainContext == null)
            {
                plainContext = new PlainContext();
                context.Features.Set<PlainContext>(plainContext);
            }

            return plainContext;
        }
    }
}

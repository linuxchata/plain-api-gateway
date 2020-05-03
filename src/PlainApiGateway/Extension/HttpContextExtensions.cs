using System;

using Microsoft.AspNetCore.Http;

using PlainApiGateway.Domain.Entity.Http;

namespace PlainApiGateway.Extension
{
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Extension method for creating the <see cref="PlainHttpContext"/> for the current HTTP context.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/> context.</param>
        /// <returns>The <see cref="PlainHttpContext"/>.</returns>
        public static PlainHttpContext CreatePlainHttpContext(this HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var plainHttpContext = context.Features.Get<PlainHttpContext>();
            if (plainHttpContext != null)
            {
                throw new InvalidOperationException("Plain HTTP context was already create for given HTTP context");
            }

            plainHttpContext = new PlainHttpContext();
            context.Features.Set(plainHttpContext);

            return plainHttpContext;
        }

        /// <summary>
        /// Extension method for getting the <see cref="PlainHttpContext"/> from the current HTTP context.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/> context.</param>
        /// <returns>The <see cref="PlainHttpContext"/>.</returns>
        public static PlainHttpContext GetPlainHttpContext(this HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var plainHttpContext = context.Features.Get<PlainHttpContext>();
            if (plainHttpContext == null)
            {
                throw new InvalidOperationException("Plain HTTP context cannot be null for given HTTP context");
            }

            return plainHttpContext;
        }
    }
}

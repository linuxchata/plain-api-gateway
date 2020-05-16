using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace PlainApiGateway.Configuration
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class PlainMiddlewareConfiguration
    {
        /// <summary>
        /// Custom pre request middleware
        /// </summary>
        public Func<HttpContext, Func<Task>, Task> PreRequestMiddleware { get; set; }

        /// <summary>
        /// Custom pre response middleware
        /// </summary>
        public Func<HttpContext, Func<Task>, Task> PreResponseMiddleware { get; set; }
    }
}
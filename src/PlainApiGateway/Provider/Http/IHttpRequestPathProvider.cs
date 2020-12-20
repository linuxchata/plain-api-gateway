namespace PlainApiGateway.Provider.Http
{
    public interface IHttpRequestPathProvider
    {
        /// <summary>
        /// Gets request path
        /// </summary>
        /// <param name="httpRequestPath">HTTP request path</param>
        /// <param name="sourcePathTemplate">Source path template</param>
        /// <param name="targetPathTemplate">Target path template</param>
        /// <returns>The request path</returns>
        string Get(string httpRequestPath, string sourcePathTemplate, string targetPathTemplate);
    }
}
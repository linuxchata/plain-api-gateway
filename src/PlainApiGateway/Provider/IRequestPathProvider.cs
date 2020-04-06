namespace PlainApiGateway.Provider
{
    public interface IRequestPathProvider
    {
        /// <summary>
        /// Gets request path
        /// </summary>
        /// <param name="httpRequestPath">HTTP request path</param>
        /// <param name="routeTargetPath">Target path</param>
        /// <returns>Returns request path</returns>
        string GetPath(string httpRequestPath, string routeTargetPath);
    }
}
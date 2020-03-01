namespace PlainApiGateway.Provider
{
    public interface IRequestPathProvider
    {
        string GetPath(string httpRequestPath, string routeTargetPath);
    }
}
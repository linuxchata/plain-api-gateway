using System;
using System.Text.RegularExpressions;

using PlainApiGateway.Constant;

namespace PlainApiGateway.Provider
{
    public sealed class RequestPathProvider : IRequestPathProvider
    {
        public string GetPath(string httpRequestPath, string routeTargetPath)
        {
            if (string.IsNullOrWhiteSpace(httpRequestPath))
            {
                throw new ArgumentNullException(nameof(httpRequestPath));
            }

            if (string.IsNullOrWhiteSpace(routeTargetPath))
            {
                throw new ArgumentNullException(nameof(routeTargetPath));
            }

            var basePath = Regex.Replace(routeTargetPath, RoutePath.VariableRegex, string.Empty);

            var basePathUri = basePath.TrimEnd('/');
            var requestPathUri = httpRequestPath.TrimStart('/');
            return $"{basePathUri}/{requestPathUri}";
        }
    }
}

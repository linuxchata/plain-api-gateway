using System;
using System.Linq;
using System.Text.RegularExpressions;

using PlainApiGateway.Constant;

namespace PlainApiGateway.Provider
{
    public sealed class RequestPathProvider : IRequestPathProvider
    {
        public string GetPath(string httpRequestPath, string routeSourcePath, string routeTargetPath)
        {
            if (string.IsNullOrWhiteSpace(httpRequestPath))
            {
                throw new ArgumentNullException(nameof(httpRequestPath));
            }

            if (string.IsNullOrWhiteSpace(routeSourcePath))
            {
                throw new ArgumentNullException(nameof(routeSourcePath));
            }

            if (string.IsNullOrWhiteSpace(routeTargetPath))
            {
                throw new ArgumentNullException(nameof(routeTargetPath));
            }

            string sourcePathTemplateRegex = Regex.Replace(routeSourcePath, RoutePath.VariableRegex, RoutePath.AnyCharacterRegex, RegexOptions.Compiled);

            var matches = Regex.Matches(httpRequestPath, sourcePathTemplateRegex, RegexOptions.Compiled);
            var groups = matches[0].Groups.Values.Select(a => a.Value).Skip(1).ToList();
            string requestPathUri = string.Empty;
            foreach (var group in groups)
            {
                requestPathUri = Regex.Replace(routeTargetPath, RoutePath.VariableRegex, group, RegexOptions.Compiled);
            }

            return requestPathUri;
        }
    }
}

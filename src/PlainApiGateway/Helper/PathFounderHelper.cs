using System.Text.RegularExpressions;

using Microsoft.AspNetCore.Http;

using PlainApiGateway.Constant;

namespace PlainApiGateway.Helper
{
    public static class PathFounderHelper
    {
        public static bool IsMatch(string sourcePathTemplate, PathString requestPath)
        {
            if (sourcePathTemplate is null)
            {
                return false;
            }

            //Transforms /v{version}/post/{any} to /v(.*)/post/(.*)
            var sourcePathTemplateRegex = Regex.Replace(
                sourcePathTemplate,
                RoutePath.VariableRegex,
                RoutePath.AnyCharacterRegex,
                RegexOptions.Compiled);

            var matches = Regex.Matches(requestPath, sourcePathTemplateRegex, RegexOptions.Compiled);

            return matches.Count != 0;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using PlainApiGateway.Constant;

namespace PlainApiGateway.Provider
{
    public sealed class RequestPathProvider : IRequestPathProvider
    {
        public string Get(string httpRequestPath, string sourcePathTemplate, string targetPathTemplate)
        {
            if (string.IsNullOrWhiteSpace(httpRequestPath))
            {
                throw new ArgumentNullException(nameof(httpRequestPath));
            }

            if (string.IsNullOrWhiteSpace(sourcePathTemplate))
            {
                throw new ArgumentNullException(nameof(sourcePathTemplate));
            }

            if (string.IsNullOrWhiteSpace(targetPathTemplate))
            {
                throw new ArgumentNullException(nameof(targetPathTemplate));
            }

            var pathVariables = GetPathVariables(httpRequestPath, sourcePathTemplate);

            string requestPathUri = string.Empty;
            foreach (var pathVariable in pathVariables)
            {
                requestPathUri = ReplacePathTemplateWithVariable(targetPathTemplate, pathVariable);
            }

            return requestPathUri;
        }

        private static List<string> GetPathVariables(string httpRequestPath, string sourcePathTemplate)
        {
            string sourcePathTemplateRegex = GetSearchPathTemplateVariablesRegex(sourcePathTemplate);

            var matches = GetPathVariablesMatches(httpRequestPath, sourcePathTemplateRegex);

            var variables = GetVariables(matches); // Skip full match

            return variables.ToList();
        }

        private static string GetSearchPathTemplateVariablesRegex(string sourcePathTemplate)
        {
            return Regex.Replace(sourcePathTemplate, RoutePath.VariableRegex, RoutePath.AnyCharacterRegex, RegexOptions.Compiled);
        }

        private static MatchCollection GetPathVariablesMatches(string httpRequestPath, string sourcePathTemplateRegex)
        {
            return Regex.Matches(httpRequestPath, sourcePathTemplateRegex, RegexOptions.Compiled);
        }

        private static IEnumerable<string> GetVariables(MatchCollection matches)
        {
            return matches[0].Groups.Values.Select(a => a.Value).Skip(1);
        }

        private static string ReplacePathTemplateWithVariable(string targetPathTemplate, string pathVariable)
        {
            return Regex.Replace(targetPathTemplate, RoutePath.VariableRegex, pathVariable, RegexOptions.Compiled);
        }
    }
}

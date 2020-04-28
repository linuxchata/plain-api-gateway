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

            string sourcePathTemplateRegex = GetSearchPathTemplateVariablesRegex(sourcePathTemplate);

            var matches = GetPathVariablesMatches(httpRequestPath, sourcePathTemplateRegex);
            if (!matches.Any())
            {
                throw new ArgumentException(nameof(httpRequestPath));
            }

            if (string.Equals(matches[0].Value, httpRequestPath))
            {
                return httpRequestPath;
            }

            var pathVariables = GetPathVariables(matches);

            string requestPathUri = string.Empty;
            foreach (var pathVariable in pathVariables)
            {
                requestPathUri = ReplacePathTemplateWithVariable(targetPathTemplate, pathVariable);
            }

            return requestPathUri;
        }

        private static string GetSearchPathTemplateVariablesRegex(string sourcePathTemplate)
        {
            return Regex.Replace(sourcePathTemplate, RoutePath.VariableRegex, RoutePath.AnyCharacterRegex, RegexOptions.Compiled);
        }

        private static MatchCollection GetPathVariablesMatches(string httpRequestPath, string sourcePathTemplateRegex)
        {
            return Regex.Matches(httpRequestPath, sourcePathTemplateRegex, RegexOptions.Compiled);
        }

        private static List<string> GetPathVariables(MatchCollection matches)
        {
            return matches[0].Groups.Values
                .Select(a => a.Value)
                .Where(a => !string.IsNullOrEmpty(a))
                .Skip(1) // Skip full match
                .ToList();
        }

        private static string ReplacePathTemplateWithVariable(string targetPathTemplate, string pathVariable)
        {
            return Regex.Replace(targetPathTemplate, RoutePath.VariableRegex, pathVariable, RegexOptions.Compiled);
        }
    }
}

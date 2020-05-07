using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using PlainApiGateway.Constant;

namespace PlainApiGateway.Provider.Http
{
    public sealed class HttpRequestPathProvider : IHttpRequestPathProvider
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

            var searchSourcePathTemplateVariablesRegex = GetSearchPathTemplateVariablesRegex(sourcePathTemplate);

            var matches = GetPathTemplateVariablesMatches(httpRequestPath, searchSourcePathTemplateVariablesRegex);

            var sourcePathTemplateVariables = GetPathVariables(matches);

            var requestPath = string.Empty;
            if (!sourcePathTemplateVariables.Any())
            {
                requestPath = RemoveVariablePlaceholderFromTargetPathTemplate(targetPathTemplate);
                return requestPath;
            }

            foreach (var pathVariable in sourcePathTemplateVariables)
            {
                requestPath = ReplaceVariableInPathTemplate(targetPathTemplate, pathVariable);
            }

            return requestPath;
        }

        private static string GetSearchPathTemplateVariablesRegex(string sourcePathTemplate)
        {
            return Regex.Replace(sourcePathTemplate, RoutePath.VariableRegex, RoutePath.AnyCharacterRegex, RegexOptions.Compiled);
        }

        private static MatchCollection GetPathTemplateVariablesMatches(string httpRequestPath, string sourcePathTemplateRegex)
        {
            var matches = Regex.Matches(httpRequestPath, sourcePathTemplateRegex, RegexOptions.Compiled);
            if (!matches.Any())
            {
                throw new ArgumentException(nameof(httpRequestPath));
            }

            return matches;
        }

        private static List<string> GetPathVariables(MatchCollection matches)
        {
            return matches[0].Groups.Values
                .Select(a => a.Value)
                .Where(a => !string.IsNullOrEmpty(a))
                .Skip(1) // Skip full match
                .ToList();
        }

        private static string RemoveVariablePlaceholderFromTargetPathTemplate(string targetPathTemplate)
        {
            return Regex.Replace(targetPathTemplate, RoutePath.VariableRegex, string.Empty, RegexOptions.Compiled);
        }

        private static string ReplaceVariableInPathTemplate(string targetPathTemplate, string pathVariable)
        {
            return Regex.Replace(targetPathTemplate, RoutePath.VariableRegex, pathVariable, RegexOptions.Compiled);
        }
    }
}

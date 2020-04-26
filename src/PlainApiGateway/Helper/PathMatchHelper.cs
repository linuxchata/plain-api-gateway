using System;
using System.Text.RegularExpressions;

using Microsoft.AspNetCore.Http;

using PlainApiGateway.Constant;

namespace PlainApiGateway.Helper
{
    public static class PathMatchHelper
    {
        public static bool IsMatch(string sourcePathTemplate, PathString requestPath)
        {
            var matches = Regex.Matches(sourcePathTemplate, RoutePath.Any, RegexOptions.Compiled);
            if (IsExactPathMatch(matches))
            {
                return string.Equals(sourcePathTemplate, requestPath, StringComparison.OrdinalIgnoreCase);
            }

            if (IsAnyPathMatch(matches, sourcePathTemplate))
            {
                return true;
            }

            if (IsAnyWithPrefixPathMatch(sourcePathTemplate, requestPath, matches))
            {
                return true;
            }

            return false;
        }

        private static bool IsExactPathMatch(MatchCollection matches)
        {
            return matches.Count == 0;
        }

        private static bool IsAnyPathMatch(MatchCollection matches, string sourcePathTemplate)
        {
            var sourcePathTemplateWithoutSlashes = sourcePathTemplate.Trim('/');

            return matches.Count == 1 &&
                matches[0].Success &&
                string.Equals(matches[0].Value, sourcePathTemplateWithoutSlashes, StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsAnyWithPrefixPathMatch(string sourcePathTemplate, PathString requestPath, MatchCollection matches)
        {
            var sourcePathTemplatePrefixEndIndex = sourcePathTemplate.IndexOf("/{", StringComparison.Ordinal);
            var sourcePathTemplatePrefix = sourcePathTemplate.Substring(0, sourcePathTemplatePrefixEndIndex);

            return matches.Count == 1 &&
                matches[0].Success &&
                requestPath.Value.StartsWith(sourcePathTemplatePrefix);
        }
    }
}
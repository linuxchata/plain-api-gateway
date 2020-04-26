using System;
using System.Text.RegularExpressions;

using Microsoft.AspNetCore.Http;

using PlainApiGateway.Constant;

namespace PlainApiGateway.Helper
{
    public static class PathMatchHelper
    {
        public static bool IsMatch(string sourcePath, PathString requestPath)
        {
            var matches = Regex.Matches(sourcePath, RoutePath.Any, RegexOptions.Compiled);
            if (matches.Count == 0)
            {
                return sourcePath == requestPath;
            }

            if (matches.Count == 1 && matches[0].Success)
            {
                return true;
            }

            throw new ArgumentException($"Source path {sourcePath} is invalid");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.Primitives;

namespace PlainApiGateway.Domain.Entity.Http
{
    public sealed class PlainHttpHeader
    {
        public PlainHttpHeader(string key, IEnumerable<string> values)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            this.Key = key;
            this.Value = new StringValues(values.ToArray());
        }

        public string Key { get; }

        public StringValues Value { get; }
    }
}
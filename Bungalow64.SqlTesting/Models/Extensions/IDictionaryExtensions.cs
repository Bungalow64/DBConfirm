using Microsoft.Data.SqlClient;
using Models.Exceptions;
using Models.Templates.Asbtract;
using Models.Templates.Placeholders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Models.Extensions
{
    public static class IDictionaryExtensions
    {
        public static SqlParameter[] ToSqlParameters(this IDictionary<string, object> dictionary)
        {
            if ((dictionary?.Count ?? 0) == 0)
            {
                return new SqlParameter[0];
            }

            object getValue(KeyValuePair<string, object> value)
            {
                if (value.Value is RequiredPlaceholder)
                {
                    throw new RequiredPlaceholderIsNullException($"The value for {value.Key} is required but has not been set", value.Key);
                }
                if (value.Value is IResolver resolverValue)
                {
                    return resolverValue.Resolve();
                }

                return value.Value ?? DBNull.Value;
            };

            return dictionary.Select(p => new SqlParameter($"@{p.Key}", getValue(p))).ToArray();
        }
    }
}

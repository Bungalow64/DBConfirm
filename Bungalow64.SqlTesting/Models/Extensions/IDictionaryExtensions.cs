using Microsoft.Data.SqlClient;
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
            object getValue(KeyValuePair<string, object> value)
            {
                if (value.Value is RequiredPlaceholder)
                {
                    // TODO: Confirm correct exception, use different or custom
                    throw new MissingMemberException($"The value for {value.Key} is required but has not been set");
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

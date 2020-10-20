using Microsoft.Data.SqlClient;
using Models.Exceptions;
using Models.Templates.Abstract;
using Models.Templates.Placeholders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Models.Extensions
{
    /// <summary>
    /// Defines extensions to <see cref="IDictionary{TKey, TValue}"/>
    /// </summary>
    public static class IDictionaryExtensions
    {
        /// <summary>
        /// Converts an <see cref="IDictionary{TKey, TValue}"/> to an array of <see cref="SqlParameter"/> objects.  The Key is used as the column name, and the Value the parameter value
        /// </summary>
        /// <param name="dictionary">The dictionary to convert.  A null or empty dictionary results in an empty array</param>
        /// <returns>Returns an array of <see cref="SqlParameter"/> objects</returns>
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

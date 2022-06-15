using DBConfirm.Core.Exceptions;
using DBConfirm.Core.Templates.Abstract;
using DBConfirm.Core.Templates.Placeholders;
using DBConfirm.Databases.MySQL.Exceptions;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DBConfirm.Databases.MySQL.Extensions
{
    /// <summary>
    /// Defines extensions to <see cref="IDictionary{TKey, TValue}"/>
    /// </summary>
    public static class IDictionaryExtensions
    {
        /// <summary>
        /// Converts an <see cref="IDictionary{TKey, TValue}"/> to an IList of <see cref="MySqlParameter"/> objects.  The Key is used as the column name, and the Value the parameter value
        /// </summary>
        /// <param name="dictionary">The dictionary to convert.  A null or empty dictionary results in an empty array</param>
        /// <param name="tableName">The name of the table being inserted into, for more specific error messages</param>
        /// <returns>Returns an IList of <see cref="MySqlParameter"/> objects</returns>
        public static IList<MySqlParameter> ToSqlParameters(this IDictionary<string, object> dictionary, string tableName)
        {
            if ((dictionary?.Count ?? 0) == 0)
            {
                return new MySqlParameter[0];
            }

            object getValue(KeyValuePair<string, object> value)
            {
                if (value.Value is RequiredPlaceholder)
                {
                    if (string.IsNullOrWhiteSpace(tableName))
                    {
                        throw new RequiredPlaceholderIsNullException(value.Key);
                    }
                    else
                    {
                        throw new RequiredPlaceholderIsNullException(value.Key, tableName);
                    }
                }
                if (value.Value is IResolver resolverValue)
                {
                    return resolverValue.Resolve();
                }

                return value.Value ?? DBNull.Value;
            };

            return dictionary.Select(p => new MySqlParameter($"@{p.Key}", getValue(p))).ToList();
        }

        /// <summary>
        /// Converts an <see cref="IDictionary{TKey, TValue}"/> to an IList of <see cref="MySqlParameter"/> objects.  The Key is used as the column name, and the Value the parameter value
        /// </summary>
        /// <param name="dictionary">The dictionary to convert.  A null or empty dictionary results in an empty array</param>
        /// <returns>Returns an IList of <see cref="MySqlParameter"/> objects</returns>
        public static IList<MySqlParameter> ToSqlParameters(this IDictionary<string, object> dictionary)
        {
            return dictionary.ToSqlParameters(null);
        }
    }
}

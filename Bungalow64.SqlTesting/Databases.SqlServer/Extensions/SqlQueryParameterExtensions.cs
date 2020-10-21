using Microsoft.Data.SqlClient;
using SQLConfirm.Core.Exceptions;
using SQLConfirm.Core.Parameters;
using SQLConfirm.Core.Templates.Abstract;
using SQLConfirm.Core.Templates.Placeholders;
using System;
using System.Linq;

namespace SQLConfirm.Databases.SqlServer.Extensions
{
    /// <summary>
    /// Defines extensions to <see cref="SqlQueryParameter"/>
    /// </summary>
    public static class SqlQueryParameterExtensions
    {
        /// <summary>
        /// Converts an array of <see cref="SqlQueryParameter"/> to an array of <see cref="SqlParameter"/> objects
        /// </summary>
        /// <param name="parameters">The array to convert.  A null or empty array results in an empty array</param>
        /// <returns>Returns an array of <see cref="SqlParameter"/> objects</returns>
        public static SqlParameter[] ToSqlParameters(this SqlQueryParameter[] parameters)
        {
            if ((parameters?.Length ?? 0) == 0)
            {
                return new SqlParameter[0];
            }

            object getValue(SqlQueryParameter value)
            {
                if (value.Value is RequiredPlaceholder)
                {
                    throw new RequiredPlaceholderIsNullException($"The value for {value.ParameterName} is required but has not been set", value.ParameterName);
                }
                if (value.Value is IResolver resolverValue)
                {
                    return resolverValue.Resolve();
                }

                return value.Value ?? DBNull.Value;
            };

            return parameters.Select(p => new SqlParameter($"@{p.ParameterName}", getValue(p))).ToArray();
        }
    }
}

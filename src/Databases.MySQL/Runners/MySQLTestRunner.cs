using DBConfirm.Core.Data;
using DBConfirm.Core.DataResults;
using DBConfirm.Core.Parameters;
using DBConfirm.Core.Runners.Abstract;
using DBConfirm.Core.Templates;
using DBConfirm.Core.Templates.Abstract;
using DBConfirm.Core.TestFrameworks.Abstract;
using DBConfirm.Databases.MySQL.Exceptions;
using DBConfirm.Databases.MySQL.Extensions;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace DBConfirm.Databases.MySQL.Runners
{
    /// <summary>
    /// The MySQL test runner, handling all SQL connections for a single database.  When communicating with a database multiple times within a single test, the same test runner instance must be used.
    /// </summary>
    public class MySQLTestRunner : ITestRunner
    {
        #region Setup

        private string ConnectionString { get; }
        private MySqlConnection SqlConnection { get; set; }
        private MySqlTransaction SqlTransaction { get; set; }

        private ITestFramework _testFramework;

        private bool disposedValue;

        /// <summary>
        /// Constructor, setting the connection string of the target database
        /// </summary>
        /// <param name="connectionString">The connection string to use</param>
        public MySQLTestRunner(string connectionString)
        {
            ConnectionString = connectionString;
        }

        /// <inheritdoc/>
        public async Task InitialiseAsync(ITestFramework testFramework)
        {
            _testFramework = testFramework ?? throw new ArgumentNullException(nameof(testFramework));

            SqlConnection = new MySqlConnection(ConnectionString);
            await SqlConnection.OpenAsync();

            SqlTransaction = SqlConnection.BeginTransaction(Debugger.IsAttached ? IsolationLevel.ReadUncommitted : IsolationLevel.ReadCommitted);
        }

        private void DisposeConnections()
        {
            void Run(Action act)
            {
                try
                {
                    act();
                }
                catch (Exception) { }
            }
            Run(() => SqlTransaction.Rollback());
            Run(() => SqlTransaction.Dispose());
            Run(() => SqlConnection.Close());
            Run(() => SqlConnection.Dispose());
        }

        #endregion

        #region Actions

        private async Task ExecuteStoredProcedureNonQueryAsync(string procedureName, IList<MySqlParameter> parameters)
        {
            using (MySqlCommand command = new MySqlCommand(procedureName, SqlConnection, SqlTransaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddRange(parameters.ToArray());

                await command.ExecuteNonQueryAsync();
            }
        }

        /// <inheritdoc/>
        public Task ExecuteStoredProcedureNonQueryAsync(string procedureName, IDictionary<string, object> parameters)
        {
            return ExecuteStoredProcedureNonQueryAsync(procedureName, parameters.ToSqlParameters());
        }

        /// <inheritdoc/>
        public Task ExecuteStoredProcedureNonQueryAsync(string procedureName, params SqlQueryParameter[] parameters)
        {
            return ExecuteStoredProcedureNonQueryAsync(procedureName, parameters.ToSqlParameters());
        }

        private Task<QueryResult> ExecuteStoredProcedureQueryAsync(string procedureName, IList<MySqlParameter> parameters)
        {
            using (DataSet ds = new DataSet())
            {
                ds.Locale = CultureInfo.InvariantCulture;

                using (MySqlCommand command = new MySqlCommand(procedureName, SqlConnection, SqlTransaction))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parameters.ToArray());

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        adapter.Fill(ds);
                    }
                }
                if (ds.Tables.Count > 0)
                {
                    return Task.FromResult(new QueryResult(_testFramework, ds.Tables[0]));
                }
                return Task.FromResult(new QueryResult(_testFramework));
            }
        }

        /// <inheritdoc/>
        public Task<QueryResult> ExecuteStoredProcedureQueryAsync(string procedureName, IDictionary<string, object> parameters)
        {
            return ExecuteStoredProcedureQueryAsync(procedureName, parameters.ToSqlParameters());
        }

        /// <inheritdoc/>
        public Task<QueryResult> ExecuteStoredProcedureQueryAsync(string procedureName, SqlQueryParameter[] parameters)
        {
            return ExecuteStoredProcedureQueryAsync(procedureName, parameters.ToSqlParameters());
        }

        /// <inheritdoc/>
        public Task<QueryResult> ExecuteViewAsync(string viewName)
        {
            return ExecuteCommandAsync($"SELECT * FROM {DelimitTableName(viewName)}");
        }

        /// <inheritdoc/>
        public async Task<int> CountRowsInViewAsync(string viewName)
        {
            long total = (await ExecuteCommandScalarAsync<long>($"SELECT COUNT(*) AS `Count` FROM {DelimitTableName(viewName)}")).RawData;

            return Convert.ToInt32(total);
        }

        private Task<IList<QueryResult>> ExecuteStoredProcedureMultipleDataSetAsync(string procedureName, IList<MySqlParameter> parameters)
        {
            using (DataSet ds = new DataSet())
            {
                ds.Locale = CultureInfo.InvariantCulture;

                using (MySqlCommand command = new MySqlCommand(procedureName, SqlConnection, SqlTransaction))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parameters.ToArray());

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        adapter.Fill(ds);
                    }
                }
                return Task.FromResult((IList<QueryResult>)ds.Tables.Cast<DataTable>().Select(p => new QueryResult(_testFramework, p)).ToList());
            }
        }

        /// <inheritdoc/>
        public Task<IList<QueryResult>> ExecuteStoredProcedureMultipleDataSetAsync(string procedureName, IDictionary<string, object> parameters)
        {
            return ExecuteStoredProcedureMultipleDataSetAsync(procedureName, parameters.ToSqlParameters());
        }

        /// <inheritdoc/>
        public Task<IList<QueryResult>> ExecuteStoredProcedureMultipleDataSetAsync(string procedureName, params SqlQueryParameter[] parameters)
        {
            return ExecuteStoredProcedureMultipleDataSetAsync(procedureName, parameters.ToSqlParameters());
        }

        private async Task<ScalarResult<T>> ExecuteStoredProcedureScalarAsync<T>(string procedureName, IList<MySqlParameter> parameters)
        {
            using (MySqlCommand command = new MySqlCommand(procedureName, SqlConnection, SqlTransaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddRange(parameters.ToArray());

                return new ScalarResult<T>(_testFramework, (T)await command.ExecuteScalarAsync());
            }
        }

        /// <inheritdoc/>
        public Task<ScalarResult<T>> ExecuteStoredProcedureScalarAsync<T>(string procedureName, IDictionary<string, object> parameters)
        {
            return ExecuteStoredProcedureScalarAsync<T>(procedureName, parameters.ToSqlParameters());
        }

        /// <inheritdoc/>
        public Task<ScalarResult<T>> ExecuteStoredProcedureScalarAsync<T>(string procedureName, params SqlQueryParameter[] parameters)
        {
            return ExecuteStoredProcedureScalarAsync<T>(procedureName, parameters.ToSqlParameters());
        }

        private async Task ExecuteCommandNoResultsAsync(string commandText, IList<MySqlParameter> parameters)
        {
            using (MySqlCommand command = new MySqlCommand(commandText, SqlConnection, SqlTransaction))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddRange(parameters.ToArray());

                await command.ExecuteNonQueryAsync();
            }
        }

        /// <inheritdoc/>
        public Task ExecuteCommandNoResultsAsync(string commandText, IDictionary<string, object> parameters)
        {
            return ExecuteCommandNoResultsAsync(commandText, parameters.ToSqlParameters());
        }

        /// <inheritdoc/>
        public Task ExecuteCommandNoResultsAsync(string commandText, params SqlQueryParameter[] parameters)
        {
            return ExecuteCommandNoResultsAsync(commandText, parameters.ToSqlParameters());
        }

        private Task<QueryResult> ExecuteCommandAsync(string commandText, IList<MySqlParameter> parameters)
        {
            using (DataSet ds = new DataSet())
            {
                ds.Locale = CultureInfo.InvariantCulture;

                using (MySqlCommand command = new MySqlCommand(commandText, SqlConnection, SqlTransaction))
                {
                    command.Parameters.AddRange(parameters.ToArray());

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        adapter.Fill(ds);
                    }
                }
                if (ds.Tables.Count > 0)
                {
                    return Task.FromResult(new QueryResult(_testFramework, ds.Tables[0]));
                }
                return Task.FromResult(new QueryResult(_testFramework));
            }
        }

        /// <inheritdoc/>
        public Task<QueryResult> ExecuteCommandAsync(string commandText, IDictionary<string, object> parameters)
        {
            return ExecuteCommandAsync(commandText, parameters?.ToSqlParameters() ?? new MySqlParameter[0]);
        }

        /// <inheritdoc/>
        public Task<QueryResult> ExecuteCommandAsync(string commandText, params SqlQueryParameter[] parameters)
        {
            return ExecuteCommandAsync(commandText, parameters?.ToSqlParameters() ?? new MySqlParameter[0]);
        }

        private async Task<ScalarResult<T>> ExecuteCommandScalarAsync<T>(string commandText, IList<MySqlParameter> parameters)
        {
            using (MySqlCommand command = new MySqlCommand(commandText, SqlConnection, SqlTransaction))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddRange(parameters.ToArray());
                return new ScalarResult<T>(_testFramework, (T)await command.ExecuteScalarAsync());
            }
        }

        /// <inheritdoc/>
        public Task<ScalarResult<T>> ExecuteCommandScalarAsync<T>(string commandText, IDictionary<string, object> parameters)
        {
            return ExecuteCommandScalarAsync<T>(commandText, parameters.ToSqlParameters());
        }

        /// <inheritdoc/>
        public Task<ScalarResult<T>> ExecuteCommandScalarAsync<T>(string commandText, params SqlQueryParameter[] parameters)
        {
            return ExecuteCommandScalarAsync<T>(commandText, parameters.ToSqlParameters());
        }

        private Task<IList<QueryResult>> ExecuteCommandMultipleDataSetAsync(string commandText, IList<MySqlParameter> parameters)
        {
            using (DataSet ds = new DataSet())
            {
                ds.Locale = CultureInfo.InvariantCulture;

                using (MySqlCommand command = new MySqlCommand(commandText, SqlConnection, SqlTransaction))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddRange(parameters.ToArray());

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        adapter.Fill(ds);
                    }
                }
                return Task.FromResult((IList<QueryResult>)ds.Tables.Cast<DataTable>().Select(p => new QueryResult(_testFramework, p)).ToList());
            }
        }

        /// <inheritdoc/>
        public Task<IList<QueryResult>> ExecuteCommandMultipleDataSetAsync(string commandText, IDictionary<string, object> parameters)
        {
            return ExecuteCommandMultipleDataSetAsync(commandText, parameters.ToSqlParameters());
        }

        /// <inheritdoc/>
        public Task<IList<QueryResult>> ExecuteCommandMultipleDataSetAsync(string commandText, params SqlQueryParameter[] parameters)
        {
            return ExecuteCommandMultipleDataSetAsync(commandText, parameters.ToSqlParameters());
        }

        /// <inheritdoc/>
        public Task<QueryResult> ExecuteTableAsync(string tableName)
        {
            return ExecuteViewAsync(tableName);
        }

        /// <inheritdoc/>
        public Task<int> CountRowsInTableAsync(string tableName)
        {
            return CountRowsInViewAsync(tableName);
        }

        /// <inheritdoc/>
        public Task<T> InsertTemplateAsync<T>() where T : ITemplate, new()
        {
            return InsertTemplateAsync(new T());
        }

        /// <inheritdoc/>
        public async Task<T> InsertTemplateAsync<T>(T template) where T : ITemplate
        {
            if (template.IsInserted)
            {
                return template;
            }

            await template.InsertAsync(this);

            template.RecordInsertion();
            return template;
        }

        /// <inheritdoc/>
        public Task<ITemplate> InsertTemplateAsync(ITemplate template)
        {
            return InsertTemplateAsync<ITemplate>(template);
        }

        /// <inheritdoc/>
        public Task<DataSetRow> InsertDefaultAsync(string tableName)
        {
            return InsertDataAsync(tableName, new DataSetRow());
        }

        /// <inheritdoc/>
        public Task<DataSetRow> InsertDataAsync(string tableName, DataSetRow data)
        {
            return InsertDataAsync(tableName, data, null);
        }

        /// <inheritdoc/>
        public async Task<DataSetRow> InsertDataAsync(string tableName, DataSetRow defaultData, DataSetRow overrideData)
        {
            tableName = DelimitTableName(tableName);

            DataSetRow data = defaultData.Copy();
            if (overrideData != null)
            {
                data.Apply(overrideData);
            }

            string tableCheck = $@"
                SELECT
                    COUNT(*)
                FROM 
                    INFORMATION_SCHEMA.TABLES
                WHERE
                    TABLE_SCHEMA = Database()
                    AND TABLE_TYPE = 'BASE TABLE' 
                    AND TABLE_NAME = @TableNameDBConfirmProperty;";

            long foundTables = (await ExecuteCommandScalarAsync<long>(tableCheck, new SqlQueryParameter("TableNameDBConfirmProperty", tableName.Trim('`')))).RawData;
            
            if (foundTables == 0)
            {
                throw new TableNotFoundException(tableName);
            }

            string identityCheck = $@"
                SELECT
                    COLUMN_NAME
                FROM
                    INFORMATION_SCHEMA.COLUMNS 
                WHERE
                    TABLE_SCHEMA = Database()
                    AND TABLE_NAME = @TableNameDBConfirmProperty
                    AND EXTRA LIKE '%auto_increment%'
                ORDER BY ORDINAL_POSITION ASC
                LIMIT 1;";

            string identityColumn = (await ExecuteCommandScalarAsync<string>(identityCheck, new SqlQueryParameter("TableNameDBConfirmProperty", tableName.Trim('`')))).RawData;

            bool hasIdentity = !string.IsNullOrWhiteSpace(identityColumn);

            bool isInsertingIdentity = hasIdentity && data.Any(p => DelimitName(p.Key).Trim('`').Equals(identityColumn, StringComparison.OrdinalIgnoreCase));

            string command = $@"
                INSERT INTO
                    {tableName}
                (
                    { string.Join(",", data.Select(p => DelimitName(p.Key))) }
                )
                VALUES
                (
                    { string.Join(",", data.Select(p => $"@{p.Key}")) }
                );

                SELECT LAST_INSERT_ID();";

            try
            {
                IList<MySqlParameter> parameters = data.ToSqlParameters(tableName);

                ulong newId = (await ExecuteCommandScalarAsync<ulong>(command, parameters)).RawData;

                if (!hasIdentity || isInsertingIdentity || newId == 0)
                {
                    return data;
                }

                data[identityColumn] = Convert.ToInt32(newId);

                return data;
            }
            catch (TableNotFoundException ex)
            {
                _testFramework.Fail(ex.Message);
                return null;
            }
        }

        private string DelimitTableName(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new InvalidOperationException("The name of the table cannot be null or whitespace");
            }

            string[] parts = tableName.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Any(p => p.Trim().Trim('`').Trim('`').Length > 64))
            {
                throw new InvalidOperationException("The name of the table or schema cannot be more than 64 characters");
            }

            return string.Join(".", parts.Select(DelimitName));
        }

        /// <inheritdoc/>
        public int GenerateNextIdentity() => CustomIdentityService.GenerateNextIdentity();

        private static string DelimitName(string name)
        {
            name = name.Trim(' ').TrimStart('`').TrimEnd('`');
            return $"`{name}`";
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Rolls back the active transaction, and closes the connection to the target database
        /// </summary>
        /// <param name="disposing">Indicates whether the object is being disposed from the <see cref="IDisposable.Dispose"/> method</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    DisposeConnections();
                }

                disposedValue = true;
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}

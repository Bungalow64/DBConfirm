using System;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using System.Data;
using System.Globalization;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using SQLConfirm.Core.DataResults;
using SQLConfirm.Core.TestFrameworks.Abstract;
using SQLConfirm.Core.Templates.Abstract;
using SQLConfirm.Core.Runners.Abstract;
using SQLConfirm.Core.Data;
using SQLConfirm.Core.Parameters;
using SQLConfirm.Databases.SQLServer.Extensions;

namespace SQLConfirm.Databases.SQLServer.Runners
{
    /// <summary>
    /// The SQL Server test runner, handling all SQL connections for a single database.  When communicating with a database multiple times within a single test, the same test runner instance must be used.
    /// </summary>
    public class SQLServerTestRunner : ITestRunner
    {
        #region Setup

        private string ConnectionString { get; }
        private SqlConnection SqlConnection { get; set; }
        private SqlTransaction SqlTransaction { get; set; }

        private ITestFramework _testFramework;

        private bool disposedValue;

        /// <summary>
        /// Constructor, setting the connection string of the target database
        /// </summary>
        /// <param name="connectionString">The connection string to use</param>
        public SQLServerTestRunner(string connectionString)
        {
            ConnectionString = connectionString;
        }

        /// <inheritdoc/>
        public async Task InitialiseAsync(ITestFramework testFramework)
        {
            _testFramework = testFramework ?? throw new ArgumentNullException(nameof(testFramework));

            SqlConnection = new SqlConnection(ConnectionString);
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

        private async Task ExecuteStoredProcedureNonQueryAsync(string procedureName, SqlParameter[] parameters)
        {
            using (SqlCommand command = new SqlCommand(procedureName, SqlConnection, SqlTransaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddRange(parameters);

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

        private Task<QueryResult> ExecuteStoredProcedureQueryAsync(string procedureName, SqlParameter[] parameters)
        {
            using (DataSet ds = new DataSet())
            {
                ds.Locale = CultureInfo.InvariantCulture;

                using (SqlCommand command = new SqlCommand(procedureName, SqlConnection, SqlTransaction))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parameters);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
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
            return ExecuteCommandAsync($"SELECT * FROM {viewName}");
        }

        /// <inheritdoc/>
        public async Task<int> CountRowsInViewAsync(string viewName)
        {
            return (await ExecuteCommandScalarAsync<int>($"SELECT COUNT(*) AS [Count] FROM {viewName}")).RawData;
        }

        private Task<IList<QueryResult>> ExecuteStoredProcedureMultipleDataSetAsync(string procedureName, SqlParameter[] parameters)
        {
            using (DataSet ds = new DataSet())
            {
                ds.Locale = CultureInfo.InvariantCulture;

                using (SqlCommand command = new SqlCommand(procedureName, SqlConnection, SqlTransaction))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parameters);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
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

        private async Task<ScalarResult<T>> ExecuteStoredProcedureScalarAsync<T>(string procedureName, SqlParameter[] parameters)
        {
            using (SqlCommand command = new SqlCommand(procedureName, SqlConnection, SqlTransaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddRange(parameters);

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

        private async Task ExecuteCommandNoResultsAsync(string commandText, SqlParameter[] parameters)
        {
            using (SqlCommand command = new SqlCommand(commandText, SqlConnection, SqlTransaction))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddRange(parameters);

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

        private Task<QueryResult> ExecuteCommandAsync(string commandText, SqlParameter[] parameters)
        {
            using (DataSet ds = new DataSet())
            {
                ds.Locale = CultureInfo.InvariantCulture;

                using (SqlCommand command = new SqlCommand(commandText, SqlConnection, SqlTransaction))
                {
                    command.Parameters.AddRange(parameters);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
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
            return ExecuteCommandAsync(commandText, parameters?.ToSqlParameters() ?? new SqlParameter[0]);
        }

        /// <inheritdoc/>
        public Task<QueryResult> ExecuteCommandAsync(string commandText, params SqlQueryParameter[] parameters)
        {
            return ExecuteCommandAsync(commandText, parameters?.ToSqlParameters() ?? new SqlParameter[0]);
        }

        private async Task<ScalarResult<T>> ExecuteCommandScalarAsync<T>(string commandText, SqlParameter[] parameters)
        {
            using (SqlCommand command = new SqlCommand(commandText, SqlConnection, SqlTransaction))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddRange(parameters);

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

        private Task<IList<QueryResult>> ExecuteCommandMultipleDataSetAsync(string commandText, SqlParameter[] parameters)
        {
            using (DataSet ds = new DataSet())
            {
                ds.Locale = CultureInfo.InvariantCulture;

                using (SqlCommand command = new SqlCommand(commandText, SqlConnection, SqlTransaction))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddRange(parameters);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
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
        public Task<DataSetRow> InsertDataAsync(string tableName, DataSetRow data)
        {
            return InsertDataAsync(tableName, data, null);
        }

        /// <inheritdoc/>
        public async Task<DataSetRow> InsertDataAsync(string tableName, DataSetRow defaultData, DataSetRow overrideData)
        {
            DataSetRow data = defaultData;
            if (overrideData != null)
            {
                data = data.Merge(overrideData);
            }

            if (data.All(p => p.Value == null))
            {
                (int? defaultIdentityValue, string defaultIdentityColumnName) = await InsertDefaultWithIdentity(tableName);

                return new DataSetRow
                {
                    [defaultIdentityColumnName] = defaultIdentityValue
                };
            }

            string command = $@"
                DECLARE @HasIdentity BIT = 0;
                DECLARE @IdentityColumnNameUpdated BIT = 0;
                DECLARE @IdentityColumnName NVARCHAR(255) = '';
                IF OBJECTPROPERTY(OBJECT_ID('{tableName}'), 'TableHasIdentity') = 1
                BEGIN
	                SELECT TOP (1)
                        @HasIdentity = 1
		                ,@IdentityColumnName = name
	                FROM 
		                sys.identity_columns
	                WHERE
		                OBJECT_SCHEMA_NAME(object_id) + '.' + OBJECT_NAME(object_id) = '{tableName}'
                    OR
		                '[' + OBJECT_SCHEMA_NAME(object_id) + '].' + OBJECT_NAME(object_id) = '{tableName}'
                    OR
		                OBJECT_SCHEMA_NAME(object_id) + '.[' + OBJECT_NAME(object_id) + ']' = '{tableName}'
                    OR
		                '[' + OBJECT_SCHEMA_NAME(object_id) + '].[' + OBJECT_NAME(object_id) + ']' = '{tableName}'
                END

                IF (@HasIdentity = 1)
                BEGIN

                    DECLARE @Columns TABLE (ColumnName NVARCHAR(255))
                    INSERT INTO
	                    @Columns
                    VALUES
	                    ({ string.Join("),(", data.Select(p => $"'{DelimitColumnName(p.Key)}'")) })

                    IF (EXISTS(SELECT * FROM @Columns WHERE ColumnName = '[' + @IdentityColumnName + ']'))
                    BEGIN
                        SET @IdentityColumnNameUpdated = 1;
                    END

	                IF (@IdentityColumnNameUpdated = 1)
	                BEGIN
		                SET IDENTITY_INSERT {tableName} ON;
	                END
                END

                INSERT INTO
                    {tableName}
                (
                    { string.Join(",", data.Select(p => DelimitColumnName(p.Key))) }
                )
                VALUES
                (
                    { string.Join(",", data.Select(p => $"@{p.Key}")) }
                );

                IF (@HasIdentity = 1)
                BEGIN
	                IF (@IdentityColumnNameUpdated = 1)
	                BEGIN
		                SET IDENTITY_INSERT {tableName} OFF;
	                END
	                ELSE
	                BEGIN
		                SELECT 
			                SCOPE_IDENTITY() AS IdentityValue
			                ,@IdentityColumnName AS IdentityColumnName
	                END
                END";

            QueryResult results = await ExecuteCommandAsync(command, data.ToSqlParameters());

            if (results.TotalRows == 0)
            {
                return data;
            }

            int insertedIdentityValue = Convert.ToInt32(results.RawData.Rows[0]["IdentityValue"]);
            string insertedIdentityColumnName = results.RawData.Rows[0]["IdentityColumnName"].ToString();

            (overrideData ?? data)[insertedIdentityColumnName] = insertedIdentityValue;

            return data;
        }

        private static string DelimitColumnName(string name)
        {
            name = name.TrimStart('[').TrimEnd(']');
            return $"[{name}]";
        }

        private async Task<(int? identity, string identityColumnName)> InsertDefaultWithIdentity(string tableName)
        {
            string command = $@"
                INSERT INTO
                    {tableName}
                DEFAULT VALUES;

                IF OBJECTPROPERTY(OBJECT_ID('{tableName}'), 'TableHasIdentity') = 1
                BEGIN
                    SELECT
                        SCOPE_IDENTITY() AS IdentityValue
                        ,name AS IdentityColumnName
                    FROM 
		                sys.identity_columns
	                WHERE
		                OBJECT_SCHEMA_NAME(object_id) + '.' + OBJECT_NAME(object_id) = '{tableName}'
                END
                ELSE
                BEGIN
                    SELECT NULL
                END";

            QueryResult data = await ExecuteCommandAsync(command);

            if (data.TotalRows == 0)
            {
                return (null, null);
            }
            return ((int?)data.RawData.Rows[0]["IdentityValue"], data.RawData.Rows[0]["IdentityColumnName"].ToString());
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

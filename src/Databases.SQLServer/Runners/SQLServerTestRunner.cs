using System;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using System.Data;
using System.Globalization;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using DBConfirm.Core.DataResults;
using DBConfirm.Core.TestFrameworks.Abstract;
using DBConfirm.Core.Templates.Abstract;
using DBConfirm.Core.Runners.Abstract;
using DBConfirm.Core.Data;
using DBConfirm.Core.Parameters;
using DBConfirm.Databases.SQLServer.Extensions;
using DBConfirm.Core.Templates;
using DBConfirm.Core.Exceptions;
using DBConfirm.Databases.SQLServer.Runners.Abstract;
using DBConfirm.Databases.SQLServer.Results;
using DBConfirm.Core.DataResults.Abstract;
using DBConfirm.Databases.SQLServer.ExecutionPlans.Factories.Abstract;
using DBConfirm.Databases.SQLServer.ExecutionPlans.Factories;

namespace DBConfirm.Databases.SQLServer.Runners
{
    /// <summary>
    /// The SQL Server test runner, handling all SQL connections for a single database.  When communicating with a database multiple times within a single test, the same test runner instance must be used.
    /// </summary>
    public class SQLServerTestRunner : ITestRunner, ISQLServerExecutionPlanRunner
    {
        #region Setup

        private string ConnectionString { get; }
        private SqlConnection SqlConnection { get; set; }
        private SqlTransaction SqlTransaction { get; set; }

        private ITestFramework _testFramework;

        internal IExecutionPlanFactory ExecutionPlanFactory { get; set; } = new ExecutionPlanFactory();

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

        private async Task ExecuteStoredProcedureNonQueryAsync(string procedureName, IList<SqlParameter> parameters)
        {
            using (SqlCommand command = new SqlCommand(procedureName, SqlConnection, SqlTransaction))
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

        private async Task<(QueryResult Result, IExecutionPlan ExecutionPlan)> ExecuteStoredProcedureQueryAsync(string procedureName, IList<SqlParameter> parameters, bool includeExecutionPlan = false)
        {
            (QueryResult, IExecutionPlan) Execute()
            {
                using (DataSet ds = new DataSet())
                {
                    ds.Locale = CultureInfo.InvariantCulture;

                    using (SqlCommand command = new SqlCommand(procedureName, SqlConnection, SqlTransaction))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(parameters.ToArray());

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(ds);
                        }
                    }

                    if (ds.Tables.Count > 0)
                    {
                        QueryResult result = new QueryResult(_testFramework, ds.Tables[0]);

                        IExecutionPlan plan = ExecutionPlanFactory.Build(_testFramework, ds);

                        return (result, plan);
                    }
                    return (new QueryResult(_testFramework), null);
                }
            }

            if (includeExecutionPlan)
            {
                SqlCommand clearCache = new SqlCommand("DBCC FREEPROCCACHE WITH NO_INFOMSGS;", SqlConnection, SqlTransaction);
                await clearCache.ExecuteNonQueryAsync();
                SqlCommand executionPlan = new SqlCommand("SET STATISTICS XML ON;", SqlConnection, SqlTransaction);
                await executionPlan.ExecuteNonQueryAsync();

                (QueryResult, IExecutionPlan) result = Execute();

                SqlCommand executionPlanOff = new SqlCommand("SET STATISTICS XML OFF;", SqlConnection, SqlTransaction);
                await executionPlanOff.ExecuteNonQueryAsync();

                return result;
            }
            else
            {
                return Execute();
            }
        }

        /// <inheritdoc/>
        public async Task<QueryResult> ExecuteStoredProcedureQueryAsync(string procedureName, IDictionary<string, object> parameters)
        {
            return (await ExecuteStoredProcedureQueryAsync(procedureName, parameters.ToSqlParameters())).Result;
        }

        /// <inheritdoc/>
        public async Task<QueryResult> ExecuteStoredProcedureQueryAsync(string procedureName, SqlQueryParameter[] parameters)
        {
            return (await ExecuteStoredProcedureQueryAsync(procedureName, parameters.ToSqlParameters())).Result;
        }

        /// <inheritdoc/>
        public Task<QueryResult> ExecuteViewAsync(string viewName)
        {
            return ExecuteCommandAsync($"SELECT * FROM {DelimitTableName(viewName)}");
        }

        /// <inheritdoc/>
        public async Task<int> CountRowsInViewAsync(string viewName)
        {
            return (await ExecuteCommandScalarAsync<int>($"SELECT COUNT(*) AS [Count] FROM {DelimitTableName(viewName)}")).RawData;
        }

        private Task<IList<QueryResult>> ExecuteStoredProcedureMultipleDataSetAsync(string procedureName, IList<SqlParameter> parameters)
        {
            using (DataSet ds = new DataSet())
            {
                ds.Locale = CultureInfo.InvariantCulture;

                using (SqlCommand command = new SqlCommand(procedureName, SqlConnection, SqlTransaction))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parameters.ToArray());

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

        private async Task<ScalarResult<T>> ExecuteStoredProcedureScalarAsync<T>(string procedureName, IList<SqlParameter> parameters)
        {
            using (SqlCommand command = new SqlCommand(procedureName, SqlConnection, SqlTransaction))
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

        private async Task ExecuteCommandNoResultsAsync(string commandText, IList<SqlParameter> parameters)
        {
            using (SqlCommand command = new SqlCommand(commandText, SqlConnection, SqlTransaction))
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

        private Task<QueryResult> ExecuteCommandAsync(string commandText, IList<SqlParameter> parameters)
        {
            using (DataSet ds = new DataSet())
            {
                ds.Locale = CultureInfo.InvariantCulture;

                using (SqlCommand command = new SqlCommand(commandText, SqlConnection, SqlTransaction))
                {
                    command.Parameters.AddRange(parameters.ToArray());

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

        private async Task<ScalarResult<T>> ExecuteCommandScalarAsync<T>(string commandText, IList<SqlParameter> parameters)
        {
            using (SqlCommand command = new SqlCommand(commandText, SqlConnection, SqlTransaction))
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

        private Task<IList<QueryResult>> ExecuteCommandMultipleDataSetAsync(string commandText, IList<SqlParameter> parameters)
        {
            using (DataSet ds = new DataSet())
            {
                ds.Locale = CultureInfo.InvariantCulture;

                using (SqlCommand command = new SqlCommand(commandText, SqlConnection, SqlTransaction))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddRange(parameters.ToArray());

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

            if (data.All(p => p.Value == null))
            {
                (int? defaultIdentityValue, string defaultIdentityColumnName) = await InsertDefaultWithIdentity(tableName);

                if (!defaultIdentityValue.HasValue)
                {
                    return new DataSetRow();
                }

                return new DataSetRow
                {
                    [defaultIdentityColumnName] = defaultIdentityValue
                };
            }

            string command = $@"
                DECLARE @HasIdentityDBConfirmProperty BIT = 0;
                DECLARE @IdentityColumnNameUpdatedDBConfirmProperty BIT = 0;
                DECLARE @IdentityColumnNameDBConfirmProperty NVARCHAR(255) = '';

                IF NOT EXISTS(
                    SELECT
                        *
	                FROM 
		                sys.tables
	                WHERE
		                '[' + OBJECT_SCHEMA_NAME(object_id) + '].[' + OBJECT_NAME(object_id) + ']' = @TableNameDBConfirmProperty
                )
                BEGIN
                    DECLARE @ErrorDBConfirmProperty NVARCHAR(255) = 'The table cannot be found.  Table Name: ' + @TableNameDBConfirmProperty;
	                ;THROW 51000, @ErrorDBConfirmProperty, 1;
                END

                IF OBJECTPROPERTY(OBJECT_ID(@TableNameDBConfirmProperty), 'TableHasIdentity') = 1
                BEGIN
	                SELECT TOP (1)
                        @HasIdentityDBConfirmProperty = 1
		                ,@IdentityColumnNameDBConfirmProperty = name
	                FROM 
		                sys.identity_columns
	                WHERE
		                '[' + OBJECT_SCHEMA_NAME(object_id) + '].[' + OBJECT_NAME(object_id) + ']' = @TableNameDBConfirmProperty
                END

                IF (@HasIdentityDBConfirmProperty = 1)
                BEGIN

                    DECLARE @Columns TABLE (ColumnName NVARCHAR(255))
                    INSERT INTO
	                    @Columns
                    VALUES
	                    ({ string.Join("),(", data.Select(p => $"'{DelimitName(p.Key)}'")) })

                    IF (EXISTS(SELECT * FROM @Columns WHERE ColumnName = '[' + @IdentityColumnNameDBConfirmProperty + ']'))
                    BEGIN
                        SET @IdentityColumnNameUpdatedDBConfirmProperty = 1;
                    END

	                IF (@IdentityColumnNameUpdatedDBConfirmProperty = 1)
	                BEGIN
		                SET IDENTITY_INSERT {tableName} ON;
	                END
                END

                INSERT INTO
                    {tableName}
                (
                    { string.Join(",", data.Select(p => DelimitName(p.Key))) }
                )
                VALUES
                (
                    { string.Join(",", data.Select(p => $"@{p.Key}")) }
                );

                IF (@HasIdentityDBConfirmProperty = 1)
                BEGIN
	                IF (@IdentityColumnNameUpdatedDBConfirmProperty = 1)
	                BEGIN
		                SET IDENTITY_INSERT {tableName} OFF;
	                END
	                ELSE
	                BEGIN
		                SELECT 
			                SCOPE_IDENTITY() AS IdentityValue
			                ,@IdentityColumnNameDBConfirmProperty AS IdentityColumnName
	                END
                END";

            try
            {
                IList<SqlParameter> parameters = data.ToSqlParameters(tableName);
                parameters.Add(new SqlParameter
                {
                    ParameterName = "TableNameDBConfirmProperty",
                    SqlDbType = SqlDbType.NVarChar,
                    Size = 500,
                    Value = tableName
                });

                QueryResult results = await ExecuteCommandAsync(command, parameters);

                if (results.TotalRows == 0)
                {
                    return data;
                }

                int insertedIdentityValue = Convert.ToInt32(results.RawData.Rows[0]["IdentityValue"]);
                string insertedIdentityColumnName = results.RawData.Rows[0]["IdentityColumnName"].ToString();

                data[insertedIdentityColumnName] = insertedIdentityValue;

                return data;
            }
            catch (RequiredPlaceholderIsNullException ex)
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

            if (parts.Any(p => p.Trim().Trim('[').Trim(']').Length > 128))
            {
                throw new InvalidOperationException("The name of the table or schema cannot be more than 128 characters");
            }

            return string.Join(".", parts.Select(DelimitName));
        }

        /// <inheritdoc/>
        public int GenerateNextIdentity() => CustomIdentityService.GenerateNextIdentity();

        private static string DelimitName(string name)
        {
            name = name.Trim(' ').TrimStart('[').TrimEnd(']');
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
		                '[' + OBJECT_SCHEMA_NAME(object_id) + '].[' + OBJECT_NAME(object_id) + ']' = '{tableName}'
                END";

            QueryResult data = await ExecuteCommandAsync(command);

            if (data.TotalRows == 0)
            {
                return (null, null);
            }
            return (Convert.ToInt32(data.RawData.Rows[0]["IdentityValue"]), data.RawData.Rows[0]["IdentityColumnName"].ToString());
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

        /// <inheritdoc/>
        Task<ExecutionPlanQueryResult<QueryResult>> ISQLServerExecutionPlanRunner.ExecuteCommandAsync(string commandText, IDictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        Task<ExecutionPlanQueryResult<QueryResult>> ISQLServerExecutionPlanRunner.ExecuteCommandAsync(string commandText, params SqlQueryParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        Task<ExecutionPlanQueryResult<IList<QueryResult>>> ISQLServerExecutionPlanRunner.ExecuteCommandMultipleDataSetAsync(string commandText, IDictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        Task<ExecutionPlanQueryResult<IList<QueryResult>>> ISQLServerExecutionPlanRunner.ExecuteCommandMultipleDataSetAsync(string commandText, params SqlQueryParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        Task<ExecutionPlanQueryResult<NoResult>> ISQLServerExecutionPlanRunner.ExecuteCommandNoResultsAsync(string commandText, IDictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        Task<ExecutionPlanQueryResult<NoResult>> ISQLServerExecutionPlanRunner.ExecuteCommandNoResultsAsync(string commandText, params SqlQueryParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        Task<ExecutionPlanQueryResult<ScalarResult<T>>> ISQLServerExecutionPlanRunner.ExecuteCommandScalarAsync<T>(string commandText, IDictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        Task<ExecutionPlanQueryResult<ScalarResult<T>>> ISQLServerExecutionPlanRunner.ExecuteCommandScalarAsync<T>(string commandText, params SqlQueryParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        Task<ExecutionPlanQueryResult<IList<QueryResult>>> ISQLServerExecutionPlanRunner.ExecuteStoredProcedureMultipleDataSetAsync(string procedureName, IDictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        Task<ExecutionPlanQueryResult<IList<QueryResult>>> ISQLServerExecutionPlanRunner.ExecuteStoredProcedureMultipleDataSetAsync(string procedureName, params SqlQueryParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        Task<ExecutionPlanQueryResult<NoResult>> ISQLServerExecutionPlanRunner.ExecuteStoredProcedureNonQueryAsync(string procedureName, IDictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        Task<ExecutionPlanQueryResult<NoResult>> ISQLServerExecutionPlanRunner.ExecuteStoredProcedureNonQueryAsync(string procedureName, params SqlQueryParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        async Task<ExecutionPlanQueryResult<QueryResult>> ISQLServerExecutionPlanRunner.ExecuteStoredProcedureQueryAsync(string procedureName, IDictionary<string, object> parameters)
        {
            (QueryResult result, IExecutionPlan executionPlan) = await ExecuteStoredProcedureQueryAsync(procedureName, parameters.ToSqlParameters(), includeExecutionPlan: true);
            return new ExecutionPlanQueryResult<QueryResult>(_testFramework, result, executionPlan);
        }

        /// <inheritdoc/>
        async Task<ExecutionPlanQueryResult<QueryResult>> ISQLServerExecutionPlanRunner.ExecuteStoredProcedureQueryAsync(string procedureName, params SqlQueryParameter[] parameters)
        {
            (QueryResult result, IExecutionPlan executionPlan) = await ExecuteStoredProcedureQueryAsync(procedureName, parameters.ToSqlParameters(), includeExecutionPlan: true);
            return new ExecutionPlanQueryResult<QueryResult>(_testFramework, result, executionPlan);
        }

        /// <inheritdoc/>
        Task<ExecutionPlanQueryResult<ScalarResult<T>>> ISQLServerExecutionPlanRunner.ExecuteStoredProcedureScalarAsync<T>(string procedureName, IDictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        Task<ExecutionPlanQueryResult<ScalarResult<T>>> ISQLServerExecutionPlanRunner.ExecuteStoredProcedureScalarAsync<T>(string procedureName, params SqlQueryParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        Task<ExecutionPlanQueryResult<QueryResult>> ISQLServerExecutionPlanRunner.ExecuteViewAsync(string viewName)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

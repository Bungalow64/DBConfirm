using System;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using System.Data;
using System.Globalization;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace Models
{
    public class TestRunner : IDisposable
    {
        #region Setup

        private string ConnectionString { get; }
        private SqlConnection SqlConnection { get; set; }
        private SqlTransaction SqlTransaction { get; set; }

        private bool disposedValue;

        public TestRunner(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public async Task InitialiseAsync()
        {
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

        public Task<QueryResult> GetAllRowsAsync(string table)
        {
            using (DataSet ds = new DataSet())
            {
                ds.Locale = CultureInfo.InvariantCulture;

                using (SqlCommand command = new SqlCommand($"SELECT * FROM {table}", SqlConnection, SqlTransaction))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(ds);
                    }
                }
                return Task.FromResult(new QueryResult(ds.Tables[0]));
            }
        }

        public async Task ExecuteStoredProcedureNonQueryAsync(string procedureName, params SqlParameter[] parameters)
        {
            using (SqlCommand command = new SqlCommand(procedureName, SqlConnection, SqlTransaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddRange(parameters);

                await command.ExecuteNonQueryAsync();
            }
        }

        public Task<QueryResult> ExecuteStoredProcedureQueryAsync(string procedureName, params SqlParameter[] parameters)
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
                return Task.FromResult(new QueryResult(ds.Tables[0]));
            }
        }

        public Task<QueryResult> ExecuteViewAsync(string viewName)
        {
            using (DataSet ds = new DataSet())
            {
                ds.Locale = CultureInfo.InvariantCulture;

                using (SqlCommand command = new SqlCommand($"SELECT * FROM {viewName}", SqlConnection, SqlTransaction))
                {
                    command.CommandType = CommandType.Text;

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(ds);
                    }
                }
                return Task.FromResult(new QueryResult(ds.Tables[0]));
            }
        }

        public Task<IList<QueryResult>> ExecuteStoredProcedureMultipleDataSetAsync(string procedureName, params SqlParameter[] parameters)
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
                return Task.FromResult((IList<QueryResult>)ds.Tables.Cast<DataTable>().Select(p => new QueryResult(p)).ToList());
            }
        }

        public async Task<ScalarResult<T>> ExecuteStoredProcedureScalarAsync<T>(string procedureName, params SqlParameter[] parameters)
        {
            using (SqlCommand command = new SqlCommand(procedureName, SqlConnection, SqlTransaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddRange(parameters);

                return new ScalarResult<T>((T)await command.ExecuteScalarAsync());
            }
        }

        public async Task ExecuteCommandNoResultsAsync(string commandText, SqlParameter[] parameters)
        {
            using (SqlCommand command = new SqlCommand(commandText, SqlConnection, SqlTransaction))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddRange(parameters);

                await command.ExecuteNonQueryAsync();
            }
        }

        public Task<QueryResult> ExecuteCommandAsync(string commandText, SqlParameter[] parameters)
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
                return Task.FromResult(new QueryResult(ds.Tables[0]));
            }
        }

        public async Task<ScalarResult<T>> ExecuteCommandScalarAsync<T>(string commandText, SqlParameter[] parameters)
        {
            using (SqlCommand command = new SqlCommand(commandText, SqlConnection, SqlTransaction))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddRange(parameters);

                return new ScalarResult<T>((T)await command.ExecuteScalarAsync());
            }
        }

        public Task<IList<QueryResult>> ExecuteCommandMultipleDataSetAsync(string commandText, SqlParameter[] parameters)
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
                return Task.FromResult((IList<QueryResult>)ds.Tables.Cast<DataTable>().Select(p => new QueryResult(p)).ToList());
            }
        }

        public Task<QueryResult> ExecuteTableAsync(string tableName)
        {
            return ExecuteViewAsync(tableName);
        }

        #endregion

        #region Dispose

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

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}

using System;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using System.Data;
using System.Globalization;
using System.Diagnostics;

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

        public QueryResult GetAllRows(string table)
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
                return new QueryResult(ds.Tables[0]);
            }
        }

        public async Task ExecuteStoredProcedureAsync(string procedureName, params SqlParameter[] parameters)
        {
            using (SqlCommand command = new SqlCommand(procedureName, SqlConnection, SqlTransaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddRange(parameters);

                await command.ExecuteNonQueryAsync();
            }
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

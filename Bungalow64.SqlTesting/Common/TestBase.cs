using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;

namespace Common
{
    public abstract class TestBase
    {
        protected TestRunner TestRunner;
        protected static TestContext Context { get; set; }

        [ClassInitialize(InheritanceBehavior.BeforeEachDerivedClass)]
        public static void ClassInitialise(TestContext testContext)
        {
            Context = testContext;
        }

        private static IConfiguration Configuration
        {
            get
            {
                return new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();
            }
        }

        [TestInitialize]
        public async Task Init()
        {
            TestRunner = new TestRunner(Configuration.GetConnectionString("TestDatabase"));
            await TestRunner.InitialiseAsync();
        }

        [TestCleanup]
        public void Cleanup()
        {
            TestRunner.Dispose();
        }

        public Task<QueryResult> GetAllRowsAsync(string table)
        {
            return TestRunner.GetAllRowsAsync(table);
        }

        public Task ExecuteStoredProcedureNonQueryAsync(string procedureName, params SqlParameter[] parameters)
        {
            return TestRunner.ExecuteStoredProcedureNonQueryAsync(procedureName, parameters);
        }

        public Task<QueryResult> ExecuteStoredProcedureQueryAsync(string procedureName, params SqlParameter[] parameters)
        {
            return TestRunner.ExecuteStoredProcedureQueryAsync(procedureName, parameters);
        }

        public Task<IList<QueryResult>> ExecuteStoredProcedureMultipleDataSetAsync(string procedureName, params SqlParameter[] parameters)
        {
            return TestRunner.ExecuteStoredProcedureMultipleDataSetAsync(procedureName, parameters);
        }

        public Task<ScalarResult<T>> ExecuteStoredProcedureScalarAsync<T>(string procedureName, params SqlParameter[] parameters)
        {
            return TestRunner.ExecuteStoredProcedureScalarAsync<T>(procedureName, parameters);
        }

        public Task<QueryResult> ExecuteViewAsync(string viewName)
        {
            return TestRunner.ExecuteViewAsync(viewName);
        }

        public Task<QueryResult> ExecuteTableAsync(string tableName)
        {
            return TestRunner.ExecuteTableAsync(tableName);
        }

        public Task ExecuteCommandNoResultsAsync(string commandText, params SqlParameter[] parameters)
        {
            return TestRunner.ExecuteCommandNoResultsAsync(commandText, parameters);
        }

        public Task<QueryResult> ExecuteCommandAsync(string commandText, params SqlParameter[] parameters)
        {
            return TestRunner.ExecuteCommandAsync(commandText, parameters);
        }

        public Task<IList<QueryResult>> ExecuteCommandMultipleDataSetAsync(string commandText, params SqlParameter[] parameters)
        {
            return TestRunner.ExecuteCommandMultipleDataSetAsync(commandText, parameters);
        }

        public Task<ScalarResult<T>> ExecuteCommandScalarAsync<T>(string commandText, params SqlParameter[] parameters)
        {
            return TestRunner.ExecuteCommandScalarAsync<T>(commandText, parameters);
        }
    }
}

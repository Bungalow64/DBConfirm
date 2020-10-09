using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
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
        public void Init()
        {
            TestRunner = new TestRunner(Configuration.GetConnectionString("TestDatabase"));
            TestRunner.InitialiseAsync().Wait();
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
    }
}

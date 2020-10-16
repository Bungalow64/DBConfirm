using Microsoft.Extensions.Configuration;
using Models.Abstract;
using Models.Factories;
using Models.Factories.Abstract;
using System;
using System.Threading.Tasks;

namespace Models.TestFrameworks.Abstract
{
    public abstract class BaseTestBase
    {
        private const string RunSettingsParameterName = "ConnectionString";
        private const string EnvironmentVariableName = "ConnectionString";
        private const string AppConfigConnectionStringName = "TestDatabase";

        protected ITestRunner TestRunner;

        internal ITestRunnerFactory TestRunnerFactory { private get; set; } = new TestRunnerFactory();

        protected abstract ITestFramework TestFramework { get; set; }

        protected abstract string GetParameter(string parameterName);

        private static IConfiguration Configuration
        {
            get
            {
                return new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();
            }
        }

        protected async Task BaseInit()
        {
            string connectionString =
                Environment.GetEnvironmentVariable(EnvironmentVariableName)
                ?? GetParameter(RunSettingsParameterName)
                ?? Configuration.GetConnectionString(AppConfigConnectionStringName)
                ?? null;

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                TestFramework.Error($@"Cannot find connection string.  Looked in EnvironmentVariables ('{EnvironmentVariableName}'), TestContext ('{RunSettingsParameterName}' property) and appsettings.json ('{AppConfigConnectionStringName}' connection string)");
            }

            TestRunner = await NewConnectionAsync(connectionString);
        }

        protected async Task<ITestRunner> NewConnectionAsync(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            ITestRunner testRunner = TestRunnerFactory.BuildTestRunner(connectionString);
            await testRunner.InitialiseAsync(TestFramework);
            return testRunner;
        }

        protected void BaseCleanup()
        {
            TestRunner?.Dispose();
        }
    }
}

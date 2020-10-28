using Microsoft.Extensions.Configuration;
using DBConfirm.Core.Factories.Abstract;
using DBConfirm.Core.Runners.Abstract;
using System;
using System.Threading.Tasks;

namespace DBConfirm.Core.TestFrameworks.Abstract
{
    /// <summary>
    /// The abstract framework-agnostic base class for test classes, used to manage the <see cref="ITestRunner"/> instance
    /// </summary>
    public abstract class BaseTestBase
    {
        private const string RunSettingsParameterName = "ConnectionString";
        private const string EnvironmentVariableName = "ConnectionString";
        private const string AppConfigConnectionStringName = "TestDatabase";

        /// <summary>
        /// The current instance of <see cref="ITestRunner"/>
        /// </summary>
        protected ITestRunner TestRunner;

        /// <summary>
        /// The <see cref="ITestRunnerFactory"/> to be used to generate the <see cref="ITestRunner"/> to be used
        /// </summary>
        protected abstract ITestRunnerFactory TestRunnerFactory { get; set; }

        /// <summary>
        /// The <see cref="ITestFramework"/> to be used for assertions
        /// </summary>
        protected abstract ITestFramework TestFramework { get; set; }

        /// <summary>
        /// Gets the value of the specific runtime parameter.  If no value is found, null is returned
        /// </summary>
        /// <param name="parameterName">The name of the parameter</param>
        /// <returns>The value of the parameter, or null if the parameter is not found</returns>
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

        /// <summary>
        /// The initialisation method to set up the <see cref="ITestRunner"/> for the test, and making the initial connection to the target database
        /// </summary>
        /// <returns>A task representing the asynchronous operation</returns>
        protected async Task BaseInit()
        {
            string connectionString =
                Environment.GetEnvironmentVariable(EnvironmentVariableName)
                ?? GetParameter(RunSettingsParameterName)
                ?? Configuration.GetConnectionString(AppConfigConnectionStringName)
                ?? null;

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                TestFramework.Fail($@"Cannot find connection string.  Looked in EnvironmentVariables ('{EnvironmentVariableName}'), TestContext ('{RunSettingsParameterName}' property) and appsettings.json ('{AppConfigConnectionStringName}' connection string)");
            }

            TestRunner = await NewConnectionAsync(connectionString);
        }

        /// <summary>
        /// Creates a new connection to the target database, allowing a test to communicate with other databases.  The returned <see cref="ITestRunner"/> must be disposed, so it is recommended to wrap it in a using statement
        /// </summary>
        /// <param name="connectionString">The connection string of the database to connect to</param>
        /// <returns>Returns the <see cref="ITestRunner"/> for the new database</returns>
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

        /// <summary>
        /// Disposes the current instance of <see cref="ITestRunner"/>, rolling back the transaction and closing the connection
        /// </summary>
        protected void BaseCleanup()
        {
            TestRunner?.Dispose();
        }
    }
}

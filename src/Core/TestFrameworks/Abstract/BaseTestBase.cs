using DBConfirm.Core.Attributes;
using DBConfirm.Core.Comparisons;
using DBConfirm.Core.Factories.Abstract;
using DBConfirm.Core.Runners.Abstract;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace DBConfirm.Core.TestFrameworks.Abstract
{
    /// <summary>
    /// The abstract framework-agnostic base class for test classes, used to manage the <see cref="ITestRunner"/> instance
    /// </summary>
    public abstract class BaseTestBase
    {
        private const string _defaultParameterName = "DefaultConnectionString";

        /// <summary>
        /// The name of the parameter to use for finding the connection string.  
        /// </summary>
        /// <remarks>The tests will search for a connection string in environment variables, 
        /// TestContext and appsettings.json that matches this name</remarks>
        protected virtual string ParameterName => null;

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
                    .AddJsonFile("appsettings.json", true)
                    .Build();
            }
        }

        /// <summary>
        /// The initialisation method to set up the <see cref="ITestRunner"/> for the test, and making the initial connection to the target database
        /// </summary>
        /// <returns>A task representing the asynchronous operation</returns>
        protected async Task BaseInit()
        {
            ConnectionStringNameAttribute attribute = GetType().GetCustomAttribute<ConnectionStringNameAttribute>();

            string variable =
                attribute?.ConnectionStringName
                ?? ParameterName
                ?? Configuration["DefaultConnectionStringName"]
                ?? _defaultParameterName;

            string connectionString = GetConnectionStringByName(variable);

            TestRunner = await NewConnectionByConnectionStringAsync(connectionString);
        }

        /// <summary>
        /// Creates a new connection to the target database, allowing a test to communicate with other databases.  The returned <see cref="ITestRunner"/> must be disposed, so it is recommended to wrap it in a using statement
        /// </summary>
        /// <param name="connectionString">The connection string of the database to connect to</param>
        /// <returns>Returns the <see cref="ITestRunner"/> for the new database</returns>
        protected async Task<ITestRunner> NewConnectionByConnectionStringAsync(string connectionString)
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
        /// Creates a new connection to the target database, allowing a test to communicate with other databases.  The returned <see cref="ITestRunner"/> must be disposed, so it is recommended to wrap it in a using statement
        /// </summary>
        /// <param name="connectionStringName">The name of the connection string of the database to connect to</param>
        /// <returns>Returns the <see cref="ITestRunner"/> for the new database</returns>
        protected async Task<ITestRunner> NewConnectionByConnectionStringNameAsync(string connectionStringName)
        {
            if (string.IsNullOrWhiteSpace(connectionStringName))
            {
                throw new ArgumentNullException(nameof(connectionStringName));
            }

            string connectionString = GetConnectionStringByName(connectionStringName);

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

        /// <summary>
        /// Facade to build data comparison objects, used to test comparisons with more flexibility
        /// </summary>
        protected ExpectedData Comparisons { get; } = new ExpectedData();
        
        private string GetConnectionStringByName(string connectionStringName)
        {
            string connectionString= Environment.GetEnvironmentVariable(connectionStringName)
                            ?? GetParameter(connectionStringName)
                            ?? Configuration.GetConnectionString(connectionStringName);

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                TestFramework.Fail($@"Cannot find connection string.  Looked in EnvironmentVariables ('{connectionStringName}'), TestContext ('{connectionStringName}' property) and appsettings.json ('{connectionStringName}' connection string)");
            }
            return connectionString;
        }
    }
}

using Microsoft.Extensions.Configuration;
using Models.Abstract;
using Models.Factories;
using Models.Factories.Abstract;
using System.Threading.Tasks;

namespace Models.TestFrameworks.Abstract
{
    public abstract class BaseTestBase
    {
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
            string connectionString = GetParameter("ConnectionString");

            if (connectionString == null)
            {
                connectionString = Configuration.GetConnectionString("TestDatabase");
            }

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                TestFramework.Error(@"Cannot find connection string in TestContext ('ConnectionString' property) or in appsettings.json ('ConnectionStrings\TestDatabase')");
            }

            TestRunner = TestRunnerFactory.BuildTestRunner(connectionString);
            await TestRunner.InitialiseAsync(TestFramework);
        }

        protected void BaseCleanup()
        {
            TestRunner?.Dispose();
        }
    }
}

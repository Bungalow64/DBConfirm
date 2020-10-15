using System.Threading.Tasks;
using Models.Factories;
using Models.Factories.Abstract;
using Microsoft.Extensions.Configuration;
using Models.Abstract;
using Models.TestFrameworks.Abstract;
using NUnit.Framework;

namespace Frameworks.NUnit
{
    public abstract class TestBase
    {
        protected ITestRunner TestRunner;

        protected static TestContext Context { get; set; }

        internal ITestRunnerFactory TestRunnerFactory { private get; set; } = new TestRunnerFactory();

        internal ITestFramework TestFramework { private get; set; } = new NUnitFramework();

        private static IConfiguration Configuration
        {
            get
            {
                return new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();
            }
        }

        [SetUp]
        public async Task Init()
        {
            TestRunner = TestRunnerFactory.BuildTestRunner(Configuration.GetConnectionString("TestDatabase"));
            await TestRunner.InitialiseAsync(TestFramework);
        }

        [TearDown]
        public void Cleanup()
        {
            TestRunner.Dispose();
        }
    }
}

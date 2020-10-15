using System.Threading.Tasks;
using Models.Factories;
using Models.Factories.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Abstract;
using Models.TestFrameworks.Abstract;

namespace Frameworks.MSTest2
{
    public abstract class TestBase
    {
        protected ITestRunner TestRunner;
        protected static TestContext Context { get; set; }

        internal ITestRunnerFactory TestRunnerFactory { private get; set; } = new TestRunnerFactory();

        internal ITestFramework TestFramework { private get; set; } = new MSTest2Framework();

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
            TestRunner = TestRunnerFactory.BuildTestRunner(Configuration.GetConnectionString("TestDatabase"));
            await TestRunner.InitialiseAsync(TestFramework);
        }

        [TestCleanup]
        public void Cleanup()
        {
            TestRunner.Dispose();
        }
    }
}

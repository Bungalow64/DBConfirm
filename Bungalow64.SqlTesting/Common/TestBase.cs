using System.Threading.Tasks;
using Common.Factories;
using Common.Factories.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Abstract;

namespace Common
{
    public abstract class TestBase
    {
        protected ITestRunner TestRunner;
        protected static TestContext Context { get; set; }

        internal ITestRunnerFactory TestRunnerFactory { private get; set; } = new TestRunnerFactory();

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
            await TestRunner.InitialiseAsync();
        }

        [TestCleanup]
        public void Cleanup()
        {
            TestRunner.Dispose();
        }
    }
}

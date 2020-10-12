using System.Threading.Tasks;
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
    }
}

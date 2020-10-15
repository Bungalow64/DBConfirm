using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.TestFrameworks.Abstract;

namespace Frameworks.MSTest2
{
    public abstract class TestBase : BaseTestBase
    {
        protected static TestContext Context { get; set; }

        protected override ITestFramework TestFramework { get; set; } = new MSTest2Framework();

        [ClassInitialize(InheritanceBehavior.BeforeEachDerivedClass)]
        public static void ClassInitialise(TestContext testContext)
        {
            Context = testContext;
        }
        
        protected override string GetParameter(string parameterName)
        {
            if (Context != null && Context.Properties.Contains(parameterName))
            {
                return Context.Properties[parameterName]?.ToString();
            }
            return null;
        }

        [TestInitialize]
        public Task Init() => BaseInit();

        [TestCleanup]
        public void Cleanup() => BaseCleanup();
    }
}

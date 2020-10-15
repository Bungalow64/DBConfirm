using System.Threading.Tasks;
using Models.TestFrameworks.Abstract;
using NUnit.Framework;

namespace Frameworks.NUnit
{
    public abstract class TestBase : BaseTestBase
    {
        protected override ITestFramework TestFramework { get; set; } = new NUnitFramework();

        protected override string GetParameter(string parameterName)
        {
            if (TestContext.Parameters.Exists(parameterName))
            {
                return TestContext.Parameters[parameterName];
            }
            return null;
        }

        [SetUp]
        public Task Init() => BaseInit();

        [TearDown]
        public void Cleanup() => BaseCleanup();
    }
}

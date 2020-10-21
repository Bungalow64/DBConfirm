using SQLConfirm.Core.Factories.Abstract;
using SQLConfirm.Core.Runners.Abstract;
using SQLConfirm.Frameworks.MSTest;

namespace Frameworks.MSTest.Tests.TestHelpers
{
    public class MockedTestClass : TestBase
    {
        public ITestRunner ExposedTestRunner => TestRunner;

        public ITestRunnerFactory ExposedTestRunnerFactory
        {
            get => TestRunnerFactory;
            set => TestRunnerFactory = value;
        }
    }
}

using SQLConfirm.Core.Factories.Abstract;
using SQLConfirm.Core.Runners.Abstract;
using SQLConfirm.Packages.SQLServer.MSTest;

namespace Frameworks.MSTest.Tests.TestHelpers
{
    public class MockedTestClass : MSTestBase
    {
        public ITestRunner ExposedTestRunner => TestRunner;

        public ITestRunnerFactory ExposedTestRunnerFactory
        {
            get => TestRunnerFactory;
            set => TestRunnerFactory = value;
        }
    }
}

using DBConfirm.Core.Factories.Abstract;
using DBConfirm.Core.Runners.Abstract;
using DBConfirm.Packages.SQLServer.MSTest;

namespace Frameworks.MSTest.Tests.TestHelpers;

public class MockedTestClass : MSTestBase
{
    public ITestRunner ExposedTestRunner => TestRunner;

    public ITestRunnerFactory ExposedTestRunnerFactory
    {
        get => TestRunnerFactory;
        set => TestRunnerFactory = value;
    }
}

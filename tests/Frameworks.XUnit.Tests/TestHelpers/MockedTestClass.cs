using DBConfirm.Core.Factories.Abstract;
using DBConfirm.Packages.SQLServer.XUnit;

namespace Frameworks.XUnit.Tests.TestHelpers;

public class MockedTestClass : XUnitBase
{
    public MockedTestClass(ITestRunnerFactory testRunnerFactory)
        : base(testRunnerFactory)
    {
    }
}

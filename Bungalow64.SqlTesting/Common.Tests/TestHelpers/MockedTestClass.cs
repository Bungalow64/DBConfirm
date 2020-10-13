using Models.Abstract;

namespace Common.Tests.TestHelpers
{
    public class MockedTestClass : TestBase
    {
        public ITestRunner ExposedTestRunner => TestRunner;
    }
}

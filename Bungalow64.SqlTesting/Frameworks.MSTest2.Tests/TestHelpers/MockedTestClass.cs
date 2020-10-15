using Frameworks.MSTest2;
using Models.Abstract;

namespace Frameworks.MSTest2.Tests.TestHelpers
{
    public class MockedTestClass : TestBase
    {
        public ITestRunner ExposedTestRunner => TestRunner;
    }
}

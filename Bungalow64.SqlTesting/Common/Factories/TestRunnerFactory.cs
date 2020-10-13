using Common.Factories.Abstract;
using Models;
using Models.Abstract;

namespace Common.Factories
{
    public class TestRunnerFactory : ITestRunnerFactory
    {
        public ITestRunner BuildTestRunner(string connectionString)
        {
            return new TestRunner(connectionString);
        }
    }
}

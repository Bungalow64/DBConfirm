using Models.Factories.Abstract;
using Models.Abstract;

namespace Models.Factories
{
    public class TestRunnerFactory : ITestRunnerFactory
    {
        public ITestRunner BuildTestRunner(string connectionString)
        {
            return new TestRunner(connectionString);
        }
    }
}

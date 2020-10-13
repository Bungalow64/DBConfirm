using Models.Abstract;

namespace Common.Factories.Abstract
{
    public interface ITestRunnerFactory
    {
        ITestRunner BuildTestRunner(string connectionString);
    }
}

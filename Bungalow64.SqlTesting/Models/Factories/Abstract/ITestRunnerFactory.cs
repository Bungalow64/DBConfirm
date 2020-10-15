using Models.Abstract;

namespace Models.Factories.Abstract
{
    public interface ITestRunnerFactory
    {
        ITestRunner BuildTestRunner(string connectionString);
    }
}

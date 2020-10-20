using Models.Factories.Abstract;
using Models.Runners;
using Models.Runners.Abstract;

namespace Models.Factories
{
    /// <summary>
    /// The default <see cref="ITestRunner"/> factory, using <see cref="TestRunner"/>
    /// </summary>
    public class TestRunnerFactory : ITestRunnerFactory
    {
        /// <summary>
        /// Gets an instantiation of <see cref="ITestRunner"/>, using <see cref="TestRunner"/>
        /// </summary>
        /// <param name="connectionString">The connection string to use for SQL connections</param>
        /// <returns>Returns the generated <see cref="ITestRunner"/></returns>
        public ITestRunner BuildTestRunner(string connectionString)
        {
            return new TestRunner(connectionString);
        }
    }
}

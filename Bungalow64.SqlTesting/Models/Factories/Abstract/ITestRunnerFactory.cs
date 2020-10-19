using Models.Abstract;

namespace Models.Factories.Abstract
{
    /// <summary>
    /// The interface for ITestRunner factories, to generate a new instance of <see cref="ITestRunner"/> according to the factory logic
    /// </summary>
    public interface ITestRunnerFactory
    {
        /// <summary>
        /// Gets an instantiation of <see cref="ITestRunner"/>, according to the factory logic
        /// </summary>
        /// <param name="connectionString">The connection string to use for SQL connections</param>
        /// <returns>Returns the generated <see cref="ITestRunner"/></returns>
        ITestRunner BuildTestRunner(string connectionString);
    }
}

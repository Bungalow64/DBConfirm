using DBConfirm.Core.Factories.Abstract;
using DBConfirm.Core.Runners.Abstract;
using DBConfirm.Databases.MySQL.Runners;

namespace DBConfirm.Databases.MySQL.Factories
{
    /// <summary>
    /// The default <see cref="ITestRunner"/> factory, using <see cref="MySQLTestRunnerFactory"/>
    /// </summary>
    public class MySQLTestRunnerFactory : ITestRunnerFactory
    {
        /// <summary>
        /// Gets an instantiation of <see cref="ITestRunner"/>, using <see cref="MySQLTestRunnerFactory"/>
        /// </summary>
        /// <param name="connectionString">The connection string to use for SQL connections</param>
        /// <returns>Returns the generated <see cref="ITestRunner"/></returns>
        public ITestRunner BuildTestRunner(string connectionString)
        {
            return new MySQLTestRunner(connectionString);
        }
    }
}

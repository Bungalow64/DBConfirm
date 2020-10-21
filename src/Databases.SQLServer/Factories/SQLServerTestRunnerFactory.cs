using SQLConfirm.Core.Factories.Abstract;
using SQLConfirm.Core.Runners.Abstract;
using SQLConfirm.Databases.SQLServer.Runners;

namespace SQLConfirm.Databases.SQLServer.Factories
{
    /// <summary>
    /// The default <see cref="ITestRunner"/> factory, using <see cref="SQLServerTestRunner"/>
    /// </summary>
    public class SQLServerTestRunnerFactory : ITestRunnerFactory
    {
        /// <summary>
        /// Gets an instantiation of <see cref="ITestRunner"/>, using <see cref="SQLServerTestRunner"/>
        /// </summary>
        /// <param name="connectionString">The connection string to use for SQL connections</param>
        /// <returns>Returns the generated <see cref="ITestRunner"/></returns>
        public ITestRunner BuildTestRunner(string connectionString)
        {
            return new SQLServerTestRunner(connectionString);
        }
    }
}

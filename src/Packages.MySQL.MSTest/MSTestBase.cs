using DBConfirm.Core.Factories.Abstract;
using DBConfirm.Databases.MySQL.Factories;
using DBConfirm.Databases.MySQL.Runners;
using DBConfirm.Frameworks.MSTest;

namespace DBConfirm.Packages.MySQL.MSTest
{
    /// <summary>
    /// The abstract base class for test classes using MSTest and SQL Server
    /// </summary>
    public abstract class MSTestBase : MSTestFrameworkBase
    {
        /// <summary>
        /// Uses <see cref="MySQLTestRunnerFactory"/> to generate <see cref="MySQLTestRunner"/> as the test runner
        /// </summary>
        protected override ITestRunnerFactory TestRunnerFactory { get; set; } = new MySQLTestRunnerFactory();
    }
}

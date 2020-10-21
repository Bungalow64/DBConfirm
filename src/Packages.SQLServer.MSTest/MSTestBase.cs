using SQLConfirm.Core.Factories.Abstract;
using SQLConfirm.Databases.SQLServer.Factories;
using SQLConfirm.Databases.SQLServer.Runners;
using SQLConfirm.Frameworks.MSTest;

namespace SQLConfirm.Packages.SQLServer.MSTest
{
    /// <summary>
    /// The abstract base class for test classes using MSTest and SQL Server
    /// </summary>
    public abstract class MSTestBase : MSTestFrameworkBase
    {
        /// <summary>
        /// Uses <see cref="SQLServerTestRunnerFactory"/> to generate <see cref="SQLServerTestRunner"/> as the test runner
        /// </summary>
        protected override ITestRunnerFactory TestRunnerFactory { get; set; } = new SQLServerTestRunnerFactory();
    }
}

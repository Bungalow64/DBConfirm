using DBConfirm.Core.Factories.Abstract;
using DBConfirm.Databases.SQLServer.Factories;
using DBConfirm.Databases.SQLServer.Runners;
using DBConfirm.Frameworks.MSTest;

namespace DBConfirm.Packages.SQLServer.MSTest
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

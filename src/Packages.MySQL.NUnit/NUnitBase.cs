using DBConfirm.Core.Factories.Abstract;
using DBConfirm.Databases.MySQL.Factories;
using DBConfirm.Databases.MySQL.Runners;
using DBConfirm.Frameworks.NUnit;

namespace DBConfirm.Packages.MySQL.NUnit
{
    /// <summary>
    /// The abstract base class for test classes using NUnit and SQL Server
    /// </summary>
    public abstract class NUnitBase : NUnitFrameworkBase
    {
        /// <summary>
        /// Uses <see cref="MySQLTestRunnerFactory"/> to generate <see cref="MySQLTestRunner"/> as the test runner
        /// </summary>
        protected override ITestRunnerFactory TestRunnerFactory { get; set; } = new MySQLTestRunnerFactory();
    }
}

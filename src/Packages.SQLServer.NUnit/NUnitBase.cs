using SQLConfirm.Core.Factories.Abstract;
using SQLConfirm.Databases.SQLServer.Factories;
using SQLConfirm.Databases.SQLServer.Runners;
using SQLConfirm.Frameworks.NUnit;

namespace SQLConfirm.Packages.SQLServer.NUnit
{
    /// <summary>
    /// The abstract base class for test classes using NUnit and SQL Server
    /// </summary>
    public abstract class NUnitBase : NUnitFrameworkBase
    {
        /// <summary>
        /// Uses <see cref="SQLServerTestRunnerFactory"/> to generate <see cref="SQLServerTestRunner"/> as the test runner
        /// </summary>
        protected override ITestRunnerFactory TestRunnerFactory { get; set; } = new SQLServerTestRunnerFactory();
    }
}

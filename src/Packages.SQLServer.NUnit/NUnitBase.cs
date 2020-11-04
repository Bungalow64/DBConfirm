using DBConfirm.Core.Factories.Abstract;
using DBConfirm.Databases.SQLServer.Factories;
using DBConfirm.Databases.SQLServer.Runners;
using DBConfirm.Frameworks.NUnit;

namespace DBConfirm.Packages.SQLServer.NUnit
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

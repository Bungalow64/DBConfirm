using DBConfirm.Core.Factories.Abstract;
using DBConfirm.Databases.SQLServer.Factories;
using DBConfirm.Databases.SQLServer.Runners;
using DBConfirm.Frameworks.XUnit;

namespace DBConfirm.Packages.SQLServer.XUnit
{
    /// <summary>
    /// The abstract base class for test classes using XUnit and SQL Server
    /// </summary>
    public abstract class XUnitBase : XUnitFrameworkBase
    {
        /// <summary>
        /// Uses <see cref="SQLServerTestRunnerFactory"/> to generate <see cref="SQLServerTestRunner"/> as the test runner
        /// </summary>
        protected XUnitBase() : base(new SQLServerTestRunnerFactory())
        {
        }

        /// <summary>
        /// Allows the caller to provide an <see cref="ITestRunnerFactory"/>
        /// </summary>
        protected XUnitBase(ITestRunnerFactory testRunnerFactory) : base(testRunnerFactory)
        {
        }
    }
}

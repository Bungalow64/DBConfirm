using DBConfirm.Core.Factories.Abstract;
using DBConfirm.Databases.SQLServer.Factories;
using DBConfirm.Frameworks.XUnit;

namespace DBConfirm.Packages.SQLServer.XUnit
{
    /// <summary>
    /// The abstract base class for test classes using XUnit and SQL Server
    /// </summary>
    public abstract class XUnitBase : XUnitFrameworkBase
    {
        protected XUnitBase() : base(new SQLServerTestRunnerFactory())
        {
        }
        protected XUnitBase(ITestRunnerFactory testRunnerFactory) : base(testRunnerFactory)
        {
        }
    }
}

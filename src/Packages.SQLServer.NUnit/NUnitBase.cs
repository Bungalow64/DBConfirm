using DBConfirm.Databases.SQLServer.Factories;
using DBConfirm.Frameworks.NUnit;

namespace DBConfirm.Packages.SQLServer.NUnit
{
    /// <summary>
    /// The abstract base class for test classes using NUnit and SQL Server
    /// </summary>
    public abstract class NUnitBase : NUnitFrameworkBase
    {
        protected NUnitBase() : base(new SQLServerTestRunnerFactory())
        {
        }
    }
}

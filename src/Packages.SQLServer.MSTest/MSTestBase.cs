using DBConfirm.Databases.SQLServer.Factories;
using DBConfirm.Frameworks.MSTest;

namespace DBConfirm.Packages.SQLServer.MSTest
{
    /// <summary>
    /// The abstract base class for test classes using MSTest and SQL Server
    /// </summary>
    public abstract class MSTestBase : MSTestFrameworkBase
    {
        protected MSTestBase() : base(new SQLServerTestRunnerFactory())
        {
        }
    }
}

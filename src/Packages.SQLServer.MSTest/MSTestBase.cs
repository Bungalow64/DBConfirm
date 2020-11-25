using DBConfirm.Core.Factories.Abstract;
using DBConfirm.Core.Runners.Abstract;
using DBConfirm.Databases.SQLServer.Factories;
using DBConfirm.Databases.SQLServer.Runners;
using DBConfirm.Databases.SQLServer.Runners.Abstract;
using DBConfirm.Frameworks.MSTest;
using System.Threading.Tasks;

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

        /// <summary>
        /// Provides the ability to execute commands and record the execution plans for analysis
        /// </summary>
        protected ISQLServerExecutionPlanRunner ExecutionPlanRunner { get; private set; }

        /// <summary>
        /// The initialisation method to set up the <see cref="ITestRunner"/> for the test, and making the initial connection to the target database.  Also sets up
        /// the ExecutionPlanRunner, to use the same instance.
        /// </summary>
        /// <returns>A task representing the asynchronous operation</returns>
        protected override async Task BaseInit()
        {
            await base.BaseInit();

            ExecutionPlanRunner = TestRunner as ISQLServerExecutionPlanRunner;
        }
    }
}

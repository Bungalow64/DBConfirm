using DBConfirm.Core.DataResults.Abstract;
using DBConfirm.Core.TestFrameworks.Abstract;
using System.Data;

namespace DBConfirm.Databases.SQLServer.ExecutionPlans.Factories.Abstract
{
    /// <summary>
    /// Factory for generating <see cref="IExecutionPlan"/> instances, depending on the version of SQL Server identified
    /// </summary>
    public interface IExecutionPlanFactory
    {
        /// <summary>
        /// Generates a <see cref="IExecutionPlan"/> instance, depending on the version of SQL Server identified
        /// </summary>
        /// <param name="testFramework">The current test framework</param>
        /// <param name="dataSet">The list of tables potentially containing execution plans.  Tables that do not contain execution plan data will be ignored.</param>
        /// <returns>Returns a <see cref="IExecutionPlan"/> instance</returns>
        IExecutionPlan Build(ITestFramework testFramework, DataSet dataSet);
    }
}

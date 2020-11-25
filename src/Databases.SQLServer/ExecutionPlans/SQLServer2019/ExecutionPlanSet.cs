using DBConfirm.Core.DataResults.Abstract;
using DBConfirm.Core.TestFrameworks.Abstract;
using DBConfirm.Databases.SQLServer.ExecutionPlans.SQLServer2019.Xml;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DBConfirm.Databases.SQLServer.ExecutionPlans.SQLServer2019
{
    /// <summary>
    /// The execution plan for a query run on a SQL Server 2019 database
    /// </summary>
    public class ExecutionPlanSet : IExecutionPlan
    {
        /// <summary>
        /// The collection of execution plans
        /// </summary>
        public ICollection<ExecutionPlan> ExecutionPlans { get; }

        /// <summary>
        /// The test framework to use for assertions
        /// </summary>
        internal readonly ITestFramework TestFramework;

        /// <summary>
        /// Creates a new collection of <see cref="ExecutionPlan"/> instances, populating Data with the xml provided
        /// </summary>
        /// <param name="testFramework">The test framework to use for assertions</param>
        /// <param name="xml">The IEnumerable of XML execution plans</param>
        public ExecutionPlanSet(ITestFramework testFramework, IEnumerable<string> xml)
        {
            TestFramework = testFramework;
            ExecutionPlans = xml.Select(p => new ExecutionPlan(testFramework, p)).ToList();
        }

        /// <inheritdoc/>
        public IExecutionPlan AssertKeyLookups(int expectedTotal)
        {
            TestFramework.AreEqual(expectedTotal, ExecutionPlans.Sum(q => q.PlanElements.OfType<IndexScanType>().Count(p => p.Lookup)), "Key lookup count is incorrect");
            return this;
        }
    }
}

using DBConfirm.Core.DataResults.Abstract;
using DBConfirm.Core.TestFrameworks.Abstract;

namespace DBConfirm.Databases.SQLServer.Results
{
    public class ExecutionPlanQueryResult<T> : IExecutionPlan
    {
        public ITestFramework TestFramework { get; }

        public T Result { get; }

        private readonly IExecutionPlan _executionPlan;

        private IExecutionPlan ExecutionPlan
        {
            get
            {
                if (_executionPlan == null)
                {
                    TestFramework.Fail("No execution plan was found");
                }
                return _executionPlan;
            }
        }

        public ExecutionPlanQueryResult(ITestFramework testFramework, T result, IExecutionPlan executionPlan)
        {
            TestFramework = testFramework;
            Result = result;
            _executionPlan = executionPlan;
        }

        /// <inheritdoc/>
        public IExecutionPlan AssertKeyLookups(int expectedTotal) => ExecutionPlan.AssertKeyLookups(expectedTotal);
    }
}

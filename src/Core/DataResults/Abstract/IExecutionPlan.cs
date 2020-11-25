namespace DBConfirm.Core.DataResults.Abstract
{
    /// <summary>
    /// The interface for execution plans, allowing for assertions to be made on the actual plan used for a query
    /// </summary>
    public interface IExecutionPlan
    {
        /// <summary>
        /// Asserts that the number of key lookups is correct
        /// </summary>
        /// <param name="expectedTotal">The expected number of key lookups</param>
        /// <returns>Returns the same <see cref="IExecutionPlan"/> instance</returns>
        IExecutionPlan AssertKeyLookups(int expectedTotal);
    }
}

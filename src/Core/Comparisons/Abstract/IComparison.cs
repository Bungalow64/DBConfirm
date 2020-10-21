using SQLConfirm.Core.TestFrameworks.Abstract;

namespace SQLConfirm.Core.Comparisons.Abstract
{
    /// <summary>
    /// Defines the interface for comparison objects, to implement advanced assertion logic
    /// </summary>
    public interface IComparison
    {
        /// <summary>
        /// Asserts that the comparison is met.  On failure, the test framework is used to trigger the failure
        /// </summary>
        /// <param name="testFramework">The test framework to be used for assertions</param>
        /// <param name="value">The value being tested</param>
        /// <param name="messagePrefix">The prefix of the message to be used in the failure message</param>
        void Assert(ITestFramework testFramework, object value, string messagePrefix);
        /// <summary>
        /// Validates that the comparison is met.  Instead of raising a failure, a boolean is returned
        /// </summary>
        /// <param name="value">The value being tested</param>
        /// <returns>Returns whether the valid is valid according to the comparison logic</returns>
        bool Validate(object value);
    }
}

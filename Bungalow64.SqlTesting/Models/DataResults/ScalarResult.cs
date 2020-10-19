using Models.TestFrameworks.Abstract;
using Models.Validation;

namespace Models.DataResults
{
    /// <summary>
    /// The data for a specific object
    /// </summary>
    /// <typeparam name="T">The type of the data</typeparam>
    public class ScalarResult<T>
    {
        /// <summary>
        /// The value of the result
        /// </summary>
        public T RawData { get; private set; }

        /// <summary>
        /// The test framework to use for assertions
        /// </summary>
        internal readonly ITestFramework TestFramework;

        /// <summary>
        /// Constructor, including the test framework and the query result data
        /// </summary>
        /// <param name="testFramework">The test framework to use for assertions</param>
        /// <param name="rawData">The data returned from the query execution</param>
        public ScalarResult(ITestFramework testFramework, T rawData)
        {
            TestFramework = testFramework;
            RawData = rawData;
        }

        /// <summary>
        /// Asserts that the value matches the expected value
        /// </summary>
        /// <param name="expectedValue">The expected value.  Respects <see cref="Comparisons.IComparison"/> objects</param>
        /// <returns>Returns the same <see cref="ScalarResult{T}"/> object</returns>
        public ScalarResult<T> AssertValue(object expectedValue)
        {
            ValueValidation.Assert(TestFramework, expectedValue, RawData, $"Scalar result");

            return this;
        }
    }
}

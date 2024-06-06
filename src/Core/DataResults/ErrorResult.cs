using DBConfirm.Core.TestFrameworks.Abstract;
using DBConfirm.Core.Validation;
using System;

namespace DBConfirm.Core.DataResults
{
    /// <summary>
    /// The data for a specific error
    /// </summary>
    public class ErrorResult
    {
        /// <summary>
        /// The details of the thrown error
        /// </summary>
        public Exception RawData { get; private set; }

        /// <summary>
        /// The test framework to use for assertions
        /// </summary>
        internal readonly ITestFramework TestFramework;

        /// <summary>
        /// Constructor, including the test framework and the error data
        /// </summary>
        /// <param name="testFramework">The test framework to use for assertions</param>
        /// <param name="rawData">The error returned from the query execution</param>
        public ErrorResult(ITestFramework testFramework, Exception rawData)
        {
            TestFramework = testFramework;
            RawData = rawData;
        }

        /// <summary>
        /// Asserts that an error was found
        /// </summary>
        /// <returns>Returns the same <see cref="ErrorResult"/> object</returns>
        public ErrorResult AssertError()
        {
            if (RawData is null)
            {
                TestFramework.Fail("No error was found");
                return this;
            }

            return this;
        }

        /// <summary>
        /// Asserts that the error message matches the expected value.  If no error has been found, this assertion will fail
        /// </summary>
        /// <param name="expectedValue">The expected message.  Respects <see cref="Comparisons.Abstract.IComparison"/> objects</param>
        /// <returns>Returns the same <see cref="ErrorResult"/> object</returns>
        public ErrorResult AssertMessage(object expectedValue)
        {
            if (RawData is null)
            {
                TestFramework.Fail("No error was found");
                return this;
            }
            ValueValidation.Assert(TestFramework, expectedValue, RawData?.Message, $"Error result", useDBNull: false);

            return this;
        }

        /// <summary>
        /// Asserts that the error type matches the expected type.  If no error has been found, this assertion will fail
        /// </summary>
        /// <param name="expectedType">The expected type</param>
        /// <returns>Returns the same <see cref="ErrorResult"/> object</returns>
        public ErrorResult AssertType(Type expectedType)
        {
            if (RawData is null)
            {
                TestFramework.Fail("No error was found");
                return this;
            }
            ValueValidation.Assert(TestFramework, expectedType, RawData?.GetType(), $"Error result", useDBNull: false);

            return this;
        }
    }
}

using SQLConfirm.Core.Comparisons.Abstract;
using SQLConfirm.Core.TestFrameworks.Abstract;
using System;

namespace SQLConfirm.Core.Comparisons.Strings
{
    /// <summary>
    /// Asserts that a string is of a specific length
    /// </summary>
    public class SpecificLength : IComparison
    {
        /// <summary>
        /// The expected length
        /// </summary>
        public int ExpectedLength { get; }

        /// <summary>
        /// Constructor, setting the expected length
        /// </summary>
        /// <param name="expectedLength">The expected length</param>
        public SpecificLength(int expectedLength)
        {
            if (expectedLength < 0)
            {
                throw new ArgumentException("Expected length cannot be less than 0", nameof(expectedLength));
            }
            ExpectedLength = expectedLength;
        }

        /// <inheritdoc/>
        public void Assert(ITestFramework testFramework, object value, string messagePrefix)
        {
            if (value == null)
            {
                value = string.Empty;
            }
            testFramework.IsInstanceOfType(value, typeof(string), $"{messagePrefix} is not a valid String object");

            testFramework.AreEqual(ExpectedLength, ((string)value)?.Length ?? 0, $"{messagePrefix} has an unexpected length");
        }

        /// <inheritdoc/>
        public bool Validate(object value)
        {
            if (value == null)
            {
                value = string.Empty;
            }
            if (!(value is string))
            {
                return false;
            }

            return (((string)value)?.Length ?? 0) == ExpectedLength;
        }
    }
}

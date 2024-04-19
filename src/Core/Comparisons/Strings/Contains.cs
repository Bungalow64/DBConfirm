using DBConfirm.Core.Comparisons.Abstract;
using DBConfirm.Core.TestFrameworks.Abstract;
using System;

namespace DBConfirm.Core.Comparisons.Strings
{
    /// <summary>
    /// Asserts that a value contains a specific string (case-sensitive)
    /// </summary>
    public class Contains : IComparison
    {
        /// <summary>
        /// The expected string
        /// </summary>
        public string Expected { get; }

        /// <summary>
        /// Constructor, setting the expected string
        /// </summary>
        /// <param name="expected">The expected contents</param>
        public Contains(string expected)
        {
            if (string.IsNullOrEmpty(expected))
            {
                throw new ArgumentException("Expected string cannot be null or empty", nameof(expected));
            }
            Expected = expected;
        }

        /// <inheritdoc/>
        public void Assert(ITestFramework testFramework, object value, string messagePrefix)
        {
            testFramework.IsInstanceOfType(value, typeof(string), $"{messagePrefix} is not a valid String object");

            testFramework.Contains((string)value, Expected, $"{messagePrefix} does not contain the expected string");
        }

        /// <inheritdoc/>
        public bool Validate(object value)
        {
            if (!(value is string))
            {
                return false;
            }

            return ((string)value)?.Contains(Expected) ?? false;
        }
    }
}

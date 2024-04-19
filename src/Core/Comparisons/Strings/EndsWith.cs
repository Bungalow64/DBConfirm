using DBConfirm.Core.Comparisons.Abstract;
using DBConfirm.Core.TestFrameworks.Abstract;
using System;

namespace DBConfirm.Core.Comparisons.Strings
{
    /// <summary>
    /// Asserts that a value ends with a specific string (case-sensitive)
    /// </summary>
    public class EndsWith : IComparison
    {
        /// <summary>
        /// The expected end string
        /// </summary>
        public string Expected { get; }

        /// <summary>
        /// Constructor, setting the expected end string
        /// </summary>
        /// <param name="expected">The expected end string</param>
        public EndsWith(string expected)
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

            testFramework.EndsWith((string)value, Expected, $"{messagePrefix} does not end with the expected string");
        }

        /// <inheritdoc/>
        public bool Validate(object value)
        {
            if (!(value is string))
            {
                return false;
            }

            return ((string)value)?.EndsWith(Expected) ?? false;
        }
    }
}

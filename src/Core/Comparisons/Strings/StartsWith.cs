using DBConfirm.Core.Comparisons.Abstract;
using DBConfirm.Core.TestFrameworks.Abstract;
using System;

namespace DBConfirm.Core.Comparisons.Strings
{
    /// <summary>
    /// Asserts that a value starts with a specific string (case-sensitive)
    /// </summary>
    public class StartsWith : IComparison
    {
        /// <summary>
        /// The expected start string
        /// </summary>
        public string Expected { get; }

        /// <summary>
        /// Constructor, setting the expected start string
        /// </summary>
        /// <param name="expected">The expected start string</param>
        public StartsWith(string expected)
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

            testFramework.StartsWith((string)value, Expected, $"{messagePrefix} does not start with the expected string");
        }

        /// <inheritdoc/>
        public bool Validate(object value)
        {
            if (!(value is string))
            {
                return false;
            }

            return ((string)value)?.StartsWith(Expected) ?? false;
        }
    }
}

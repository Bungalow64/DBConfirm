using DBConfirm.Core.Comparisons.Abstract;
using DBConfirm.Core.TestFrameworks.Abstract;
using System;

namespace DBConfirm.Core.Comparisons.Strings
{
    /// <summary>
    /// Asserts that a numeric value matches another, regardless of the actual types of the values
    /// </summary>
    public class LooseValue : IComparison
    {
        /// <summary>
        /// The expected value
        /// </summary>
        public object ExpectedValue { get; }

        /// <summary>
        /// Constructor, setting the expected value
        /// </summary>
        /// <param name="expectedValue">The expected value</param>
        public LooseValue(int expectedValue) : this((object)expectedValue)
        {
        }

        /// <summary>
        /// Constructor, setting the expected value
        /// </summary>
        /// <param name="expectedValue">The expected value</param>
        public LooseValue(short expectedValue) : this((object)expectedValue)
        {
        }

        /// <summary>
        /// Constructor, setting the expected value
        /// </summary>
        /// <param name="expectedValue">The expected value</param>
        public LooseValue(long expectedValue) : this((object)expectedValue)
        {
        }

        /// <summary>
        /// Constructor, setting the expected value
        /// </summary>
        /// <param name="expectedValue">The expected value</param>
        public LooseValue(decimal expectedValue) : this((object)expectedValue)
        {
        }

        /// <summary>
        /// Constructor, setting the expected value
        /// </summary>
        /// <param name="expectedValue">The expected value</param>
        public LooseValue(float expectedValue) : this((object)expectedValue)
        {
        }

        /// <summary>
        /// Constructor, setting the expected value
        /// </summary>
        /// <param name="expectedValue">The expected value</param>
        public LooseValue(byte expectedValue) : this((object)expectedValue)
        {
        }

        private LooseValue(object expectedValue)
        {
            ExpectedValue = expectedValue;
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

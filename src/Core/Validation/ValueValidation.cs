using DBConfirm.Core.Comparisons.Abstract;
using DBConfirm.Core.TestFrameworks.Abstract;
using System;

namespace DBConfirm.Core.Validation
{
    /// <summary>
    /// Static class to handle all assertions and validations
    /// </summary>
    public static class ValueValidation
    {
        /// <summary>
        /// <para>Asserts that the value is correct, according to the expected value.</para>
        /// <para>Where the expected value is a native type, object equality is used as the test.  Where the expected value is an <see cref="IComparison"/> object, the specific <see cref="IComparison"/> assertion logic is used</para>
        /// </summary>
        /// <param name="testFramework">The test framework to use for assertions</param>
        /// <param name="expectedValue">The expected value.  This can be either a native type or an <see cref="IComparison"/> object</param>
        /// <param name="value">The value to test</param>
        /// <param name="messagePrefix">The prefix of the message to use during assertion failure</param>
        public static void Assert(ITestFramework testFramework, object expectedValue, object value, string messagePrefix)
        {
            expectedValue = expectedValue ?? DBNull.Value;
            value = value ?? DBNull.Value;

            if (expectedValue is IComparison comparisonValue)
            {
                comparisonValue.Assert(testFramework, value, messagePrefix);
            }
            else
            {
                testFramework.AreEqual(expectedValue, value, $"{messagePrefix} has an unexpected value");
            }
        }

        /// <summary>
        /// <para>Validates whether the value is correct, according to the expected value.  No errors are raised during this call, so it can be used without risk of the test failing.</para>
        /// <para>Where the expected value is a native type, object equality is used as the test.  Where the expected value is an <see cref="IComparison"/> object, the specific <see cref="IComparison"/> assertion logic is used</para>
        /// </summary>
        /// <param name="expectedValue">The expected value.  This can be either a native type or an <see cref="IComparison"/> object</param>
        /// <param name="value">The value to test</param>
        /// <returns>Returns whether the value passes the assertion test</returns>
        internal static bool Validate(object expectedValue, object value)
        {
            expectedValue = expectedValue ?? DBNull.Value;
            value = value ?? DBNull.Value;

            if (expectedValue is IComparison comparisonValue)
            {
                return comparisonValue.Validate(value);
            }

            return Equals(expectedValue, value);
        }
    }
}

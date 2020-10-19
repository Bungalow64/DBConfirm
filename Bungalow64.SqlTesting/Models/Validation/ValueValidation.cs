using Models.Comparisons;
using Models.TestFrameworks.Abstract;
using System;

namespace Models.Validation
{
    public static class ValueValidation
    {
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
                testFramework.Assert.AreEqual(expectedValue, value, $"{messagePrefix} has an unexpected value");
            }
        }

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

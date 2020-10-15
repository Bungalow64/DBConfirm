using Models.Dates.Abstract;
using Models.States.Abstract;
using Models.Strings.Abstract;
using Models.TestFrameworks.Abstract;
using System;

namespace Models.Validation
{
    public static class ValueValidation
    {
        public static void Validate(ITestFramework testFramework, object expectedValue, object value, string messagePrefix)
        {
            expectedValue = expectedValue ?? DBNull.Value;
            value = value ?? DBNull.Value;

            if (expectedValue is IState stateValue)
            {
                stateValue.AssertState(testFramework, value, $"{messagePrefix} has an unexpected state");
            }
            else if (expectedValue is IDateComparison dateValue)
            {
                testFramework.Assert.IsInstanceOfType(value, typeof(DateTime), $"{messagePrefix} is not a valid DateTime object");

                dateValue.AssertDate(testFramework, (DateTime)value, $"{messagePrefix} is different by {{0}}");
            }
            else if (expectedValue is IStringComparison stringValue)
            {
                testFramework.Assert.IsInstanceOfType(value, typeof(string), $"{messagePrefix} is not a valid String object");

                stringValue.AssertString(testFramework, (string)value, $"{messagePrefix} {{0}}");
            }
            else
            {
                testFramework.Assert.AreEqual(expectedValue, value, $"{messagePrefix} has an unexpected value");
            }
        }
    }
}

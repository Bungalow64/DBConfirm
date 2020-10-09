using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Dates.Abstract;
using Models.States.Abstract;
using Models.Strings.Abstract;
using System;

namespace Models.Validation
{
    public static class ValueValidation
    {
        public static void Validate(object expectedValue, object value, string messagePrefix)
        {
            expectedValue = expectedValue ?? DBNull.Value;

            if (expectedValue is IState stateValue)
            {
                stateValue.AssertState(value, $"{messagePrefix} has an unexpected state");
            }
            else if (expectedValue is IDateComparison dateValue)
            {
                Assert.IsInstanceOfType(value, typeof(DateTime), $"{messagePrefix} is not a valid DateTime object");

                dateValue.AssertDate((DateTime)value, $"{messagePrefix} is different by {{0}}");
            }
            else if (expectedValue is IStringComparison stringValue)
            {
                Assert.IsInstanceOfType(value, typeof(string), $"{messagePrefix} is not a valid String object");

                stringValue.AssertString((string)value, $"{messagePrefix} {{0}}");
            }
            else
            {
                Assert.AreEqual(expectedValue, value, $"{messagePrefix} has an unexpected value");
            }
        }
    }
}

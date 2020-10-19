using Models.Comparisons;
using Models.TestFrameworks.Abstract;
using System;

namespace Models.Strings
{
    public class SpecificLength : IComparison
    {
        public int ExpectedLength { get; }

        public SpecificLength(int expectedLength)
        {
            if (expectedLength < 0)
            {
                throw new ArgumentException("Expected length cannot be less than 0", nameof(expectedLength));
            }
            ExpectedLength = expectedLength;
        }

        public void Assert(ITestFramework testFramework, object value, string messagePrefix)
        {
            if (value == null)
            {
                value = string.Empty;
            }
            testFramework.Assert.IsInstanceOfType(value, typeof(string), $"{messagePrefix} is not a valid String object");

            testFramework.Assert.AreEqual(ExpectedLength, ((string)value)?.Length ?? 0, $"{messagePrefix} has an unexpected length");
        }

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

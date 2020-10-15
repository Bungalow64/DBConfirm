using Models.Strings.Abstract;
using Models.TestFrameworks.Abstract;
using System;

namespace Models.Strings
{
    public class SpecificLength : IStringComparison
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

        public void AssertString(ITestFramework testFramework, string value, string message)
        {
            testFramework.Assert.AreEqual(ExpectedLength, value?.Length ?? 0, message, "has an unexpected length");
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Strings.Abstract;
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

        public void AssertString(string value, string message)
        {
            Assert.AreEqual(ExpectedLength, value?.Length ?? 0, message, "has an unexpected length");
        }
    }
}

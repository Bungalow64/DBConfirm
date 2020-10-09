using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Strings.Abstract;

namespace Models.Strings
{
    public class SpecificLength : IStringComparison
    {
        public int ExpectedLength { get; }

        public SpecificLength(int expectedLength)
        {
            ExpectedLength = expectedLength;
        }

        public void AssertString(string value, string message)
        {
            Assert.AreEqual(ExpectedLength, value?.Length ?? 0, message, "has an unexpected length");
        }
    }
}

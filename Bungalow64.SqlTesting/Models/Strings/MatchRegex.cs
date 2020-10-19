using Models.Comparisons;
using Models.TestFrameworks.Abstract;
using System;
using System.Text.RegularExpressions;

namespace Models.Strings
{
    public class MatchRegex : IComparison
    {
        public Regex ExpectedRegex { get; }

        public MatchRegex(Regex expectedRegex)
        {
            ExpectedRegex = expectedRegex ?? throw new ArgumentNullException(nameof(expectedRegex));
        }

        public MatchRegex(string expectedRegex)
        {
            if (expectedRegex == null)
            {
                throw new ArgumentNullException(nameof(expectedRegex));
            }
            ExpectedRegex = new Regex(expectedRegex);
        }

        public void Assert(ITestFramework testFramework, object value, string messagePrefix)
        {
            testFramework.Assert.IsInstanceOfType(value, typeof(string), $"{messagePrefix} is not a valid String object");

            testFramework.StringAssert.Matches((string)value, ExpectedRegex, $"{messagePrefix} does not match the regex");
        }

        public bool Validate(object value)
        {
            if (!(value is string))
            {
                return false;
            }

            return ExpectedRegex.IsMatch((string)value);
        }
    }
}

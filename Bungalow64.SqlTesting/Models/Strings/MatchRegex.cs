using Models.Strings.Abstract;
using Models.TestFrameworks.Abstract;
using System;
using System.Text.RegularExpressions;

namespace Models.Strings
{
    public class MatchRegex : IStringComparison
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

        public void AssertString(ITestFramework testFramework, string value, string message)
        {
            testFramework.StringAssert.Matches(value, ExpectedRegex, message, "does not match the regex");
        }
    }
}

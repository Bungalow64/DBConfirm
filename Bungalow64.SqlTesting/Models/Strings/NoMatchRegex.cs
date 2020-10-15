using Models.Strings.Abstract;
using Models.TestFrameworks.Abstract;
using System;
using System.Text.RegularExpressions;

namespace Models.Strings
{
    public class NoMatchRegex : IStringComparison
    {
        public Regex UnexpectedRegex { get; }

        public NoMatchRegex(Regex unexpectedRegex)
        {
            UnexpectedRegex = unexpectedRegex ?? throw new ArgumentNullException(nameof(unexpectedRegex));
        }

        public NoMatchRegex(string unexpectedRegex)
        {
            if (unexpectedRegex == null)
            {
                throw new ArgumentNullException(nameof(unexpectedRegex));
            }
            UnexpectedRegex = new Regex(unexpectedRegex);
        }

        public void AssertString(ITestFramework testFramework, string value, string message)
        {
            testFramework.StringAssert.DoesNotMatch(value, UnexpectedRegex, message, "matches the regex when it should not match");
        }
    }
}

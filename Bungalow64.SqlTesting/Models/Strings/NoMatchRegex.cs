using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Strings.Abstract;
using System.Text.RegularExpressions;

namespace Models.Strings
{
    public class NoMatchRegex : IStringComparison
    {
        public Regex UnexpectedRegex { get; }

        public NoMatchRegex(Regex unexpectedRegex)
        {
            UnexpectedRegex = unexpectedRegex;
        }

        public NoMatchRegex(string unexpectedRegex)
        {
            UnexpectedRegex = new Regex(unexpectedRegex);
        }

        public void AssertString(string value, string message)
        {
            StringAssert.DoesNotMatch(value, UnexpectedRegex, message, "matches the regex when it should not match");
        }
    }
}

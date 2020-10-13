using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Strings.Abstract;
using System;
using System.Text.RegularExpressions;

namespace Models.Strings
{
    public class MatchRegex : IStringComparison
    {
        public Regex ExpectedRegex { get; }

        public MatchRegex(Regex expectedRegex)
        {
            if (expectedRegex == null)
            {
                throw new ArgumentNullException(nameof(expectedRegex));
            }
            ExpectedRegex = expectedRegex;
        }

        public MatchRegex(string expectedRegex)
        {
            if (expectedRegex == null)
            {
                throw new ArgumentNullException(nameof(expectedRegex));
            }
            ExpectedRegex = new Regex(expectedRegex);
        }

        public void AssertString(string value, string message)
        {
            StringAssert.Matches(value, ExpectedRegex, message, "does not match the regex");
        }
    }
}

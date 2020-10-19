using Models.Comparisons;
using Models.TestFrameworks.Abstract;
using System;
using System.Text.RegularExpressions;

namespace Models.Strings
{
    public class NoMatchRegex : IComparison
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

        public void Assert(ITestFramework testFramework, object value, string messagePrefix)
        {
            testFramework.Assert.IsInstanceOfType(value, typeof(string), $"{messagePrefix} is not a valid String object");

            testFramework.StringAssert.DoesNotMatch((string)value, UnexpectedRegex, $"{messagePrefix} matches the regex when it should not match");
        }

        public bool Validate(object value)
        {
            if (!(value is string))
            {
                return false;
            }

            return !UnexpectedRegex.IsMatch((string)value);
        }
    }
}

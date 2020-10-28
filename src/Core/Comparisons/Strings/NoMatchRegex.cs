using DBConfirm.Core.Comparisons.Abstract;
using DBConfirm.Core.TestFrameworks.Abstract;
using System;
using System.Text.RegularExpressions;

namespace DBConfirm.Core.Comparisons.Strings
{
    /// <summary>
    /// Asserts that a value does not match a regex pattern
    /// </summary>
    public class NoMatchRegex : IComparison
    {
        /// <summary>
        /// The regex to not match
        /// </summary>
        public Regex UnexpectedRegex { get; }

        /// <summary>
        /// Constructor, setting the regex to not match
        /// </summary>
        /// <param name="unexpectedRegex">The regex to not match</param>
        public NoMatchRegex(Regex unexpectedRegex)
        {
            UnexpectedRegex = unexpectedRegex ?? throw new ArgumentNullException(nameof(unexpectedRegex));
        }

        /// <summary>
        /// Constructor, setting the regex to not match
        /// </summary>
        /// <param name="unexpectedRegex">The regex to not match</param>
        public NoMatchRegex(string unexpectedRegex)
        {
            if (unexpectedRegex == null)
            {
                throw new ArgumentNullException(nameof(unexpectedRegex));
            }
            UnexpectedRegex = new Regex(unexpectedRegex);
        }

        /// <inheritdoc/>
        public void Assert(ITestFramework testFramework, object value, string messagePrefix)
        {
            testFramework.IsInstanceOfType(value, typeof(string), $"{messagePrefix} is not a valid String object");

            testFramework.DoesNotMatch((string)value, UnexpectedRegex, $"{messagePrefix} matches the regex when it should not match");
        }

        /// <inheritdoc/>
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

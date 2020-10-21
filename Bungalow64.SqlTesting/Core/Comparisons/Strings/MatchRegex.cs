using SQLConfirm.Core.Comparisons.Abstract;
using SQLConfirm.Core.TestFrameworks.Abstract;
using System;
using System.Text.RegularExpressions;

namespace SQLConfirm.Core.Comparisons.Strings
{
    /// <summary>
    /// Asserts that a value matches a regex pattern
    /// </summary>
    public class MatchRegex : IComparison
    {
        /// <summary>
        /// The expected regex
        /// </summary>
        public Regex ExpectedRegex { get; }

        /// <summary>
        /// Constructor, setting the expected regex
        /// </summary>
        /// <param name="expectedRegex">The expected regex</param>
        public MatchRegex(Regex expectedRegex)
        {
            ExpectedRegex = expectedRegex ?? throw new ArgumentNullException(nameof(expectedRegex));
        }

        /// <summary>
        /// Constructor, setting the expected regex
        /// </summary>
        /// <param name="expectedRegex">The expected regex</param>
        public MatchRegex(string expectedRegex)
        {
            if (expectedRegex == null)
            {
                throw new ArgumentNullException(nameof(expectedRegex));
            }
            ExpectedRegex = new Regex(expectedRegex);
        }

        /// <inheritdoc/>
        public void Assert(ITestFramework testFramework, object value, string messagePrefix)
        {
            testFramework.IsInstanceOfType(value, typeof(string), $"{messagePrefix} is not a valid String object");

            testFramework.Matches((string)value, ExpectedRegex, $"{messagePrefix} does not match the regex");
        }

        /// <inheritdoc/>
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

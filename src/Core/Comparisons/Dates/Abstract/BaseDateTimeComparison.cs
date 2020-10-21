using SQLConfirm.Core.TestFrameworks.Abstract;
using System;

namespace SQLConfirm.Core.Comparisons.Dates.Abstract
{
    /// <summary>
    /// Abstract base class for DateTime comparisons.  Defines a default precision of 1 second
    /// </summary>
    public abstract class BaseDateTimeComparison : IDateComparison
    {
        private static TimeSpan _defaultPrecision = TimeSpan.FromSeconds(1);

        /// <inheritdoc/>
        public TimeSpan Precision { get; }

        /// <summary>
        /// Protected constructor, using the default precision of 1 second
        /// </summary>
        protected BaseDateTimeComparison() : this(_defaultPrecision) { }

        /// <summary>
        /// Protected constuctor, setting a custom precision
        /// </summary>
        /// <param name="precision">The precision to be used</param>
        protected BaseDateTimeComparison(TimeSpan precision)
        {
            Precision = precision;
        }

        /// <inheritdoc/>
        public abstract void Assert(ITestFramework testFramework, object value, string messagePrefix);

        /// <inheritdoc/>
        public abstract bool Validate(object value);

        /// <summary>
        /// Asserts that the comparison is met, within the bounds of the precision.  On failure, the test framework is used to trigger the failure
        /// </summary>
        /// <param name="testFramework">The test framework to be used for assertions</param>
        /// <param name="expectedDate">The expected date to be used in the comparison</param>
        /// <param name="value">The value being tested</param>
        /// <param name="message">The prefix of the message to be used in the failure message</param>
        protected void AssertDate(ITestFramework testFramework, DateTime expectedDate, DateTime value, string message)
        {
            TimeSpan difference = value - expectedDate;

            if (difference.Duration() >= Precision)
            {
                testFramework.AreEqual(expectedDate, value, message, $"{difference.TotalMilliseconds} ms");
            }
        }

        /// <summary>
        /// Validates that the comparison is met.  Instead of raising a failure, a boolean is returned
        /// </summary>
        /// <param name="expectedDate">The expected date to be used in the comparison</param>
        /// <param name="value">The value being tested</param>
        /// <returns>Returns whether the valid is valid according to the comparison logic</returns>
        protected bool Validate(DateTime expectedDate, DateTime value)
        {
            TimeSpan difference = value - expectedDate;

            return !(difference.Duration() >= Precision);
        }
    }
}

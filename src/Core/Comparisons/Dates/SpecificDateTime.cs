using DBConfirm.Core.Comparisons.Dates.Abstract;
using DBConfirm.Core.TestFrameworks.Abstract;
using System;

namespace DBConfirm.Core.Comparisons.Dates
{
    /// <summary>
    /// Asserts that a value matches a specific date and time.  The precision (default 1 second) is used to match values within a certain limit
    /// </summary>
    public class SpecificDateTime : BaseDateTimeComparison
    {
        /// <summary>
        /// The expected date
        /// </summary>
        public DateTime ExpectedDate { get; }

        /// <summary>
        /// Constructor, setting the expected date
        /// </summary>
        /// <param name="expectedDate">The expected date</param>
        public SpecificDateTime(DateTime expectedDate) : base()
        {
            ExpectedDate = expectedDate;
        }

        /// <summary>
        /// Constructor, setting the expected date
        /// </summary>
        /// <param name="expectedDate">The expected date, to be parsed with <see cref="DateTime.Parse(string)"/></param>
        public SpecificDateTime(string expectedDate) : this(DateTime.Parse(expectedDate)) { }

        /// <summary>
        /// Constructor, setting the expected date
        /// </summary>
        /// <param name="expectedDate">The expected date</param>
        /// <param name="precision">The precision to be used in the comparison</param>
        public SpecificDateTime(DateTime expectedDate, TimeSpan precision) : base(precision)
        {
            ExpectedDate = expectedDate;
        }

        /// <summary>
        /// Constructor, setting the expected date
        /// </summary>
        /// <param name="expectedDate">The expected date, to be parsed with <see cref="DateTime.Parse(string)"/></param>
        /// <param name="precision">The precision to be used in the comparison</param>
        public SpecificDateTime(string expectedDate, TimeSpan precision) : this(DateTime.Parse(expectedDate), precision) { }

        /// <inheritdoc/>
        public override void Assert(ITestFramework testFramework, object value, string messagePrefix)
        {
            testFramework.IsInstanceOfType(value, typeof(DateTime), $"{messagePrefix} is not a valid DateTime object");

            AssertDate(testFramework, ExpectedDate, (DateTime)value, $"{messagePrefix} is different by {{0}}");
        }

        /// <inheritdoc/>
        public override bool Validate(object value)
        {
            if (!(value is DateTime))
            {
                return false;
            }

            return Validate(ExpectedDate, (DateTime)value);
        }
    }
}

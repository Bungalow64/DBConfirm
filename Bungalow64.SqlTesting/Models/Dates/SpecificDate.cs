using Models.Dates.Abstract;
using Models.TestFrameworks.Abstract;
using System;

namespace Models.Dates
{
    /// <summary>
    /// Asserts that a value matches a specific date, ignoring the time of day.
    /// </summary>
    public class SpecificDate : IDateComparison
    {
        /// <inheritdoc/>
        public TimeSpan Precision { get; } = TimeSpan.Zero;

        /// <summary>
        /// The expected date
        /// </summary>
        public DateTime ExpectedDate { get; }

        private DateTime ExpectedDateDay => GetDateDay(ExpectedDate);

        /// <summary>
        /// Constructor, setting the expected date
        /// </summary>
        /// <param name="expectedDate">The expected date</param>
        public SpecificDate(DateTime expectedDate) : base()
        {
            ExpectedDate = expectedDate;
        }

        /// <summary>
        /// Constructor, setting the expected date
        /// </summary>
        /// <param name="expectedDate">The expected date, to be parsed with <see cref="DateTime.Parse(string)"/></param>
        public SpecificDate(string expectedDate) : this(DateTime.Parse(expectedDate)) { }

        /// <inheritdoc/>
        public void Assert(ITestFramework testFramework, object value, string messagePrefix)
        {
            testFramework.Assert.IsInstanceOfType(value, typeof(DateTime), $"{messagePrefix} is not a valid DateTime object");

            DateTime actualDay = GetDateDay((DateTime)value);

            TimeSpan difference = actualDay - ExpectedDateDay;

            testFramework.Assert.AreEqual(ExpectedDateDay,
                actualDay,
                $"{messagePrefix} is different by {{0}}",
                $"{difference.TotalDays} day{(Math.Abs(difference.TotalDays) == 1 ? "" : "s")}");
        }

        /// <inheritdoc/>
        public bool Validate(object value)
        {
            if (!(value is DateTime))
            {
                return false;
            }

            DateTime actualDay = GetDateDay((DateTime)value);

            return Equals(ExpectedDateDay, actualDay);
        }

        private static DateTime GetDateDay(DateTime value) =>
            new DateTime(value.Year, value.Month, value.Day, 0, 0, 0, value.Kind);
    }
}

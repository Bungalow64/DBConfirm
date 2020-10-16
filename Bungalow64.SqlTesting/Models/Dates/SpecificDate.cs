using Models.Dates.Abstract;
using Models.TestFrameworks.Abstract;
using System;

namespace Models.Dates
{
    public class SpecificDate : IDateComparison
    {
        public TimeSpan Precision { get; } = TimeSpan.Zero;

        public DateTime ExpectedDate { get; }

        private DateTime ExpectedDateDay => GetDateDay(ExpectedDate);

        public SpecificDate(DateTime expectedDate) : base()
        {
            ExpectedDate = expectedDate;
        }

        public SpecificDate(string expectedDate) : this(DateTime.Parse(expectedDate)) { }

        public void AssertDate(ITestFramework testFramework, DateTime value, string message)
        {
            DateTime actualDay = GetDateDay(value);

            TimeSpan difference = actualDay - ExpectedDateDay;

            testFramework.Assert.AreEqual(ExpectedDateDay,
                actualDay,
                message,
                $"{difference.TotalDays} day{(Math.Abs(difference.TotalDays) == 1 ? "" : "s")}");
        }

        public bool Validate(DateTime value)
        {
            DateTime actualDay = GetDateDay(value);

            return Equals(ExpectedDateDay, actualDay);
        }

        private static DateTime GetDateDay(DateTime value) =>
            new DateTime(value.Year, value.Month, value.Day, 0, 0, 0, value.Kind);
    }
}

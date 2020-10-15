using Models.TestFrameworks.Abstract;
using System;

namespace Models.Dates.Abstract
{
    public abstract class BaseDateTimeComparison : IDateComparison
    {
        private static TimeSpan _defaultPrecision = TimeSpan.FromSeconds(1);

        public TimeSpan Precision { get; }

        protected BaseDateTimeComparison() : this(_defaultPrecision) { }

        protected BaseDateTimeComparison(TimeSpan precision)
        {
            Precision = precision;
        }

        public abstract void AssertDate(ITestFramework testFramework, DateTime value, string message);

        protected void AssertDate(ITestFramework testFramework, DateTime expectedDate, DateTime value, string message)
        {
            TimeSpan difference = value - expectedDate;

            if (difference.Duration() >= Precision)
            {
                testFramework.Assert.AreEqual(expectedDate, value, message, $"{difference.TotalMilliseconds} ms");
            }
        }
    }
}

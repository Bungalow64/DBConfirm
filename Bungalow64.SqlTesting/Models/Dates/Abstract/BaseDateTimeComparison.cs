using Microsoft.VisualStudio.TestTools.UnitTesting;
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

        public abstract void AssertDate(DateTime value, string message);

        protected void AssertDate(DateTime expectedDate, DateTime value, string message)
        {
            TimeSpan difference = value - expectedDate;

            if (difference.Duration() >= Precision)
            {
                Assert.AreEqual(expectedDate, value, message, $"{difference.TotalMilliseconds} ms");
            }
        }
    }
}

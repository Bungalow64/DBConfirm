using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Dates.Abstract;
using System;

namespace Models.Dates
{
    public class SpecificDate : IDateComparison
    {
        public TimeSpan Precision { get; } = TimeSpan.Zero;

        public DateTime ExpectedDate { get; }

        public SpecificDate(DateTime expectedDate) : base()
        {
            ExpectedDate = expectedDate;
        }

        public SpecificDate(string expectedDate) : this(DateTime.Parse(expectedDate)) { }

        public void AssertDate(DateTime value, string message)
        {
            DateTime expectedDay = new DateTime(ExpectedDate.Year, ExpectedDate.Month, ExpectedDate.Day, 0, 0, 0, ExpectedDate.Kind);
            DateTime actualDay = new DateTime(value.Year, value.Month, value.Day, 0, 0, 0, value.Kind);

            TimeSpan difference = expectedDay - actualDay;

            Assert.AreEqual(expectedDay,
                actualDay,
                message,
                $"{difference.TotalDays} day{(difference.TotalDays == 1 ? "" : "s")}");
        }
    }
}

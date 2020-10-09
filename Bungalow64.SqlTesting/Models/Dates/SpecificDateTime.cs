using Models.Dates.Abstract;
using System;

namespace Models.Dates
{
    public class SpecificDateTime : BaseDateTimeComparison
    {
        public DateTime ExpectedDate { get; }

        public SpecificDateTime(DateTime expectedDate) : base()
        {
            ExpectedDate = expectedDate;
        }

        public SpecificDateTime(string expectedDate) : this(DateTime.Parse(expectedDate)) { }

        public SpecificDateTime(DateTime expectedDate, TimeSpan precision) : base(precision)
        {
            ExpectedDate = expectedDate;
        }

        public SpecificDateTime(string expectedDate, TimeSpan precision) : this(DateTime.Parse(expectedDate), precision) { }

        public override void AssertDate(DateTime value, string message)
        {
            AssertDate(ExpectedDate, value, message);
        }
    }
}

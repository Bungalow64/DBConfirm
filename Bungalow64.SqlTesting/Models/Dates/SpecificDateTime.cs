using Models.Dates.Abstract;
using Models.TestFrameworks.Abstract;
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

        public override void AssertDate(ITestFramework testFramework, DateTime value, string message)
        {
            AssertDate(testFramework, ExpectedDate, value, message);
        }

        public override bool Validate(DateTime value)
        {
            return Validate(ExpectedDate, value);
        }
    }
}

using Models.Dates.Abstract;
using Models.Factories;
using Models.Factories.Abstract;
using Models.TestFrameworks.Abstract;
using System;

namespace Models.Dates
{
    public class UtcNowDate : BaseDateTimeComparison
    {
        internal IDateUtcNowFactory DateUtcNowFactory { private get; set; } = new DateUtcNowFactory();

        public UtcNowDate() : base() { }

        public UtcNowDate(TimeSpan precision) : base(precision) { }

        public override void AssertDate(ITestFramework testFramework, DateTime value, string message)
        {
            AssertDate(testFramework, DateUtcNowFactory.UtcNow, value, message);
        }

        public override bool Validate(DateTime value)
        {
            return Validate(DateUtcNowFactory.UtcNow, value);
        }
    }
}

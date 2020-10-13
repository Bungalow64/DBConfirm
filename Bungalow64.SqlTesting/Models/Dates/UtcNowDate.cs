using Models.Dates.Abstract;
using Models.Factories;
using Models.Factories.Abstract;
using System;

namespace Models.Dates
{
    public class UtcNowDate : BaseDateTimeComparison
    {
        internal IDateUtcNowFactory DateUtcNowFactory { private get; set; } = new DateUtcNowFactory();

        public UtcNowDate() : base() { }

        public UtcNowDate(TimeSpan precision) : base(precision) { }

        public override void AssertDate(DateTime value, string message)
        {
            AssertDate(DateUtcNowFactory.UtcNow, value, message);
        }
    }
}

using Models.Dates.Abstract;
using System;

namespace Models.Dates
{
    public class UtcNowDate : BaseDateTimeComparison
    {
        public UtcNowDate() : base() { }

        public UtcNowDate(TimeSpan precision) : base(precision) { }

        public override void AssertDate(DateTime value, string message)
        {
            AssertDate(DateTime.UtcNow, value, message);
        }
    }
}

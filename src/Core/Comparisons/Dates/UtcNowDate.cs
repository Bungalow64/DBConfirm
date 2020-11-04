using DBConfirm.Core.Comparisons.Dates.Abstract;
using DBConfirm.Core.Factories;
using DBConfirm.Core.Factories.Abstract;
using DBConfirm.Core.TestFrameworks.Abstract;
using System;

namespace DBConfirm.Core.Comparisons.Dates
{
    /// <summary>
    /// Asserts that a value matches UtcNow.  The precision (default 1 second) is used to match values within a certain limit
    /// </summary>
    public class UtcNowDate : BaseDateTimeComparison
    {
        /// <summary>
        /// The factory used to calculate UtcNow.  By default, this is using DateTime.UtcNow
        /// </summary>
        internal IDateUtcNowFactory DateUtcNowFactory { private get; set; } = new DateUtcNowFactory();

        /// <summary>
        /// Constructor
        /// </summary>
        public UtcNowDate() : base() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="precision">The precision to be used in the comparison</param>
        public UtcNowDate(TimeSpan precision) : base(precision) { }

        /// <inheritdoc/>
        public override void Assert(ITestFramework testFramework, object value, string messagePrefix)
        {
            testFramework.IsInstanceOfType(value, typeof(DateTime), $"{messagePrefix} is not a valid DateTime object");

            AssertDate(testFramework, DateUtcNowFactory.UtcNow, (DateTime)value, $"{messagePrefix} is different by {{0}}");
        }

        /// <inheritdoc/>
        public override bool Validate(object value)
        {
            if (!(value is DateTime))
            {
                return false;
            }

            return Validate(DateUtcNowFactory.UtcNow, (DateTime)value);
        }
    }
}

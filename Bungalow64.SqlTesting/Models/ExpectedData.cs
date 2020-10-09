using System;
using Models.States;
using Models.States.Abstract;
using Models.Dates;
using Models.Dates.Abstract;
using Models.Strings.Abstract;
using Models.Strings;

namespace Models
{
    public static class ExpectedData
    {
        public static IState IsNotNull() => new NotNullState();
        public static IState IsNull() => new NullState();

        public static IDateComparison IsUtcNow() => new UtcNowDate();
        public static IDateComparison IsUtcNow(TimeSpan precision) => new UtcNowDate(precision);
        public static IDateComparison IsDateTime(string expectedDate) => new SpecificDateTime(expectedDate);
        public static IDateComparison IsDateTime(DateTime expectedDate) => new SpecificDateTime(expectedDate);
        public static IDateComparison IsDateTime(string expectedDate, TimeSpan precision) => new SpecificDateTime(expectedDate, precision);
        public static IDateComparison IsDateTime(DateTime expectedDate, TimeSpan precision) => new SpecificDateTime(expectedDate, precision);
        public static IDateComparison IsDay(string expectedDate) => new SpecificDate(expectedDate);
        public static IDateComparison IsDate(DateTime expectedDate) => new SpecificDate(expectedDate);

        public static IStringComparison HasLength(int expectedLength) => new SpecificLength(expectedLength);
    }
}

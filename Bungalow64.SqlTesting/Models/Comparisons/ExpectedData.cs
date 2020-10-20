using System;
using System.Text.RegularExpressions;
using Models.Comparisons.Abstract;
using Models.Comparisons.Strings;
using Models.Comparisons.Dates.Abstract;
using Models.Comparisons.Dates;
using Models.Comparisons.States;

namespace Models.Comparisons
{
    /// <summary>
    /// Facade to build data comparison objects, used to test comparisons with more flexibility
    /// </summary>
    public static class ExpectedData
    {
        /// <summary>
        /// Gets the state to test for not-null values
        /// </summary>
        /// <returns>Returns the comparison object</returns>
        public static IComparison IsNotNull() => new NotNullState();
        /// <summary>
        /// Gets the state to test for null values
        /// </summary>
        /// <returns>Returns the comparison object</returns>
        public static IComparison IsNull() => new NullState();

        /// <summary>
        /// Gets the state to test for the data to be UtcNow, using the default precision of 1 second
        /// </summary>
        /// <returns>Returns the comparison object</returns>
        public static IDateComparison IsUtcNow() => new UtcNowDate();
        /// <summary>
        /// Gets the state to test for the data to be UtcNow, with a custom precision
        /// </summary>
        /// <returns>Returns the comparison object</returns>
        public static IDateComparison IsUtcNow(TimeSpan precision) => new UtcNowDate(precision);
        /// <summary>
        /// Gets the state to test for the data to be a specific date, using the default precision of 1 second
        /// </summary>
        /// <returns>Returns the comparison object</returns>
        public static IDateComparison IsDateTime(string expectedDate) => new SpecificDateTime(expectedDate);
        /// <summary>
        /// Gets the state to test for the data to be a specific date, using the default precision of 1 second
        /// </summary>
        /// <returns>Returns the comparison object</returns>
        public static IDateComparison IsDateTime(DateTime expectedDate) => new SpecificDateTime(expectedDate);
        /// <summary>
        /// Gets the state to test for the data to be a specific date, with a custom precision
        /// </summary>
        /// <returns>Returns the comparison object</returns>
        public static IDateComparison IsDateTime(string expectedDate, TimeSpan precision) => new SpecificDateTime(expectedDate, precision);
        /// <summary>
        /// Gets the state to test for the data to be a specific date, with a custom precision
        /// </summary>
        /// <returns>Returns the comparison object</returns>
        public static IDateComparison IsDateTime(DateTime expectedDate, TimeSpan precision) => new SpecificDateTime(expectedDate, precision);
        /// <summary>
        /// Gets the state to test for the data to be a specific day (ignoring time), using the default precision of 1 second
        /// </summary>
        /// <returns>Returns the comparison object</returns>
        public static IDateComparison IsDay(string expectedDate) => new SpecificDate(expectedDate);
        /// <summary>
        /// Gets the state to test for the data to be a specific day (ignoring time), with a custom precision
        /// </summary>
        /// <returns>Returns the comparison object</returns>
        public static IDateComparison IsDate(DateTime expectedDate) => new SpecificDate(expectedDate);

        /// <summary>
        /// Gets the state to test for the data to be a specific length
        /// </summary>
        /// <returns>Returns the comparison object</returns>
        public static IComparison HasLength(int expectedLength) => new SpecificLength(expectedLength);
        /// <summary>
        /// Gets the state to test for the data to match a specific regex
        /// </summary>
        /// <returns>Returns the comparison object</returns>
        public static IComparison MatchesRegex(Regex expectedRegex) => new MatchRegex(expectedRegex);
        /// <summary>
        /// Gets the state to test for the data to match a specific regex
        /// </summary>
        /// <returns>Returns the comparison object</returns>
        public static IComparison MatchesRegex(string expectedRegex) => new MatchRegex(expectedRegex);
        /// <summary>
        /// Gets the state to test for the data to not match a specific regex
        /// </summary>
        /// <returns>Returns the comparison object</returns>
        public static IComparison NotMatchesRegex(Regex unexpectedRegex) => new NoMatchRegex(unexpectedRegex);
        /// <summary>
        /// Gets the state to test for the data to not match a specific regex
        /// </summary>
        /// <returns>Returns the comparison object</returns>
        public static IComparison NotMatchesRegex(string unexpectedRegex) => new NoMatchRegex(unexpectedRegex);
    }
}

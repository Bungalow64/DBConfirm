using System;
using System.Text.RegularExpressions;
using DBConfirm.Core.Comparisons.Abstract;
using DBConfirm.Core.Comparisons.Strings;
using DBConfirm.Core.Comparisons.Dates.Abstract;
using DBConfirm.Core.Comparisons.Dates;
using DBConfirm.Core.Comparisons.States;
using DBConfirm.Core.Comparisons.Types;

namespace DBConfirm.Core.Comparisons
{
    /// <summary>
    /// Facade to build data comparison objects, used to test comparisons with more flexibility
    /// </summary>
    public class ExpectedData
    {
        /// <summary>
        /// Gets the state to test for not-null values
        /// </summary>
        /// <returns>Returns the comparison object</returns>
        public IComparison IsNotNull() => new NotNullState();
        /// <summary>
        /// Gets the state to test for null values
        /// </summary>
        /// <returns>Returns the comparison object</returns>
        public IComparison IsNull() => new NullState();

        /// <summary>
        /// Gets the state to test for the data to be UtcNow, using the default precision of 1 second
        /// </summary>
        /// <returns>Returns the comparison object</returns>
        public IDateComparison IsUtcNow() => new UtcNowDate();
        /// <summary>
        /// Gets the state to test for the data to be UtcNow, with a custom precision
        /// </summary>
        /// <param name="precision">The precision to be used in the comparison</param>
        /// <returns>Returns the comparison object</returns>
        public IDateComparison IsUtcNow(TimeSpan precision) => new UtcNowDate(precision);
        /// <summary>
        /// Gets the state to test for the data to be a specific date, using the default precision of 1 second
        /// </summary>
        /// <param name="expectedDate">The expected date</param>
        /// <returns>Returns the comparison object</returns>
        public IDateComparison IsDateTime(string expectedDate) => new SpecificDateTime(expectedDate);
        /// <summary>
        /// Gets the state to test for the data to be a specific date, using the default precision of 1 second
        /// </summary>
        /// <param name="expectedDate">The expected date</param>
        /// <returns>Returns the comparison object</returns>
        public IDateComparison IsDateTime(DateTime expectedDate) => new SpecificDateTime(expectedDate);
        /// <summary>
        /// Gets the state to test for the data to be a specific date, with a custom precision
        /// </summary>
        /// <param name="expectedDate">The expected date</param>
        /// <param name="precision">The precision to be used in the comparison</param>
        /// <returns>Returns the comparison object</returns>
        public IDateComparison IsDateTime(string expectedDate, TimeSpan precision) => new SpecificDateTime(expectedDate, precision);
        /// <summary>
        /// Gets the state to test for the data to be a specific date, with a custom precision
        /// </summary>
        /// <param name="expectedDate">The expected date</param>
        /// <param name="precision">The precision to be used in the comparison</param>
        /// <returns>Returns the comparison object</returns>
        public IDateComparison IsDateTime(DateTime expectedDate, TimeSpan precision) => new SpecificDateTime(expectedDate, precision);
        /// <summary>
        /// Gets the state to test for the data to be a specific day (ignoring time), using the default precision of 1 second
        /// </summary>
        /// <param name="expectedDate">The expected date</param>
        /// <returns>Returns the comparison object</returns>
        public IDateComparison IsDay(string expectedDate) => new SpecificDate(expectedDate);
        /// <summary>
        /// Gets the state to test for the data to be a specific day (ignoring time), with a custom precision
        /// </summary>
        /// <param name="expectedDate">The expected date</param>
        /// <returns>Returns the comparison object</returns>
        public IDateComparison IsDay(DateTime expectedDate) => new SpecificDate(expectedDate);

        /// <summary>
        /// Gets the state to test for the data to be a specific length
        /// </summary>
        /// <param name="expectedLength">The expected length</param>
        /// <returns>Returns the comparison object</returns>
        public IComparison HasLength(int expectedLength) => new SpecificLength(expectedLength);
        /// <summary>
        /// Gets the state to test for the data to match a specific regex
        /// </summary>
        /// <param name="expectedRegex">The expected regex</param>
        /// <returns>Returns the comparison object</returns>
        public IComparison MatchesRegex(Regex expectedRegex) => new MatchRegex(expectedRegex);
        /// <summary>
        /// Gets the state to test for the data to match a specific regex
        /// </summary>
        /// <param name="expectedRegex">The expected regex</param>
        /// <returns>Returns the comparison object</returns>
        public IComparison MatchesRegex(string expectedRegex) => new MatchRegex(expectedRegex);
        /// <summary>
        /// Gets the state to test for the data to not match a specific regex
        /// </summary>
        /// <param name="unexpectedRegex">The regex to not match</param>
        /// <returns>Returns the comparison object</returns>
        public IComparison NotMatchesRegex(Regex unexpectedRegex) => new NoMatchRegex(unexpectedRegex);
        /// <summary>
        /// Gets the state to test for the data to not match a specific regex
        /// </summary>
        /// <param name="unexpectedRegex">The regex to not match</param>
        /// <returns>Returns the comparison object</returns>
        public IComparison NotMatchesRegex(string unexpectedRegex) => new NoMatchRegex(unexpectedRegex);
        /// <summary>
        /// Gets the state to test for the data to not match a specific regex
        /// </summary>
        /// <param name="expectedType">The expected type</param>
        /// <returns>Returns the comparison object</returns>
        public IComparison IsType(Type expectedType) => new MatchType(expectedType);
    }
}

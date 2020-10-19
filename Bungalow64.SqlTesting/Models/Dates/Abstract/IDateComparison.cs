using Models.Comparisons;
using System;

namespace Models.Dates.Abstract
{
    /// <summary>
    /// The interface for date comparison objects
    /// </summary>
    public interface IDateComparison : IComparison
    {
        /// <summary>
        /// Gets the precision to be used in the comparison
        /// </summary>
        TimeSpan Precision { get; }
    }
}

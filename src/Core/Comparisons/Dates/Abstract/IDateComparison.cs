using DBConfirm.Core.Comparisons.Abstract;
using System;

namespace DBConfirm.Core.Comparisons.Dates.Abstract
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

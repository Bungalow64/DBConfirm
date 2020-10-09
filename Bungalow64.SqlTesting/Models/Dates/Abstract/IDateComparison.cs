using System;

namespace Models.Dates.Abstract
{
    public interface IDateComparison
    {
        TimeSpan Precision { get; }

        void AssertDate(DateTime value, string message);
    }
}

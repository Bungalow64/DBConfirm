using Models.TestFrameworks.Abstract;
using System;

namespace Models.Dates.Abstract
{
    public interface IDateComparison
    {
        TimeSpan Precision { get; }

        void AssertDate(ITestFramework testFramework, DateTime value, string message);
        bool Validate(DateTime value);
    }
}

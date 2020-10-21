using SQLConfirm.Core.Comparisons.Abstract;
using SQLConfirm.Core.TestFrameworks.Abstract;
using System;

namespace SQLConfirm.Core.Comparisons.States
{
    /// <summary>
    /// Asserts that a value is not null
    /// </summary>
    public class NotNullState : IComparison
    {
        /// <inheritdoc/>
        public void Assert(ITestFramework testFramework, object value, string messagePrefix)
        {
            testFramework.AreNotEqual(DBNull.Value, value ?? DBNull.Value, $"{messagePrefix} has an unexpected state");
        }

        /// <inheritdoc/>
        public bool Validate(object value)
        {
            return DBNull.Value != (value ?? DBNull.Value);
        }
    }
}

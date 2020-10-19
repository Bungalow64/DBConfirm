using Models.Comparisons;
using Models.TestFrameworks.Abstract;
using System;

namespace Models.States
{
    /// <summary>
    /// Asserts that a value is null
    /// </summary>
    public class NullState : IComparison
    {
        /// <inheritdoc/>
        public void Assert(ITestFramework testFramework, object value, string messagePrefix)
        {
            testFramework.Assert.AreEqual(DBNull.Value, value ?? DBNull.Value, $"{messagePrefix} has an unexpected state");
        }

        /// <inheritdoc/>
        public bool Validate(object value)
        {
            return DBNull.Value == (value ?? DBNull.Value);
        }
    }
}

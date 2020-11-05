using DBConfirm.Core.Comparisons.Abstract;
using DBConfirm.Core.TestFrameworks.Abstract;
using System;

namespace DBConfirm.Core.Comparisons.Types
{
    /// <summary>
    /// Asserts that a value is of a specific type
    /// </summary>
    public class MatchType : IComparison
    {
        /// <summary>
        /// The expected type
        /// </summary>
        public Type ExpectedType { get; }

        /// <summary>
        /// Constructor, setting the expected type
        /// </summary>
        /// <param name="expectedType">The expected type</param>
        public MatchType(Type expectedType)
        {
            ExpectedType = expectedType ?? throw new ArgumentNullException(nameof(expectedType));
        }

        /// <inheritdoc/>
        public void Assert(ITestFramework testFramework, object value, string messagePrefix)
        {
            testFramework.IsInstanceOfType(value, ExpectedType, $"{messagePrefix} does not match the type");
        }

        /// <inheritdoc/>
        public bool Validate(object value)
        {
            return value?.GetType() == ExpectedType;
        }
    }
}

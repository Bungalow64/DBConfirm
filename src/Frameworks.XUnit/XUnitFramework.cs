using DBConfirm.Core.TestFrameworks.Abstract;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Xunit;

namespace DBConfirm.Frameworks.XUnit
{
    /// <summary>
    /// The test framework using XUnit
    /// </summary>
    public class XUnitFramework : ITestFramework
    {
        /// <inheritdoc/>
        public void AreEqual(object expected, object actual, string message, params string[] parameters)
        {

            Assert.Equal(expected, actual);
        }

        /// <inheritdoc/>
        public void AreNotEqual(object notExpected, object actual, string message, params string[] parameters)
        {
            Assert.NotEqual(notExpected, actual);
        }

        /// <inheritdoc/>
        public void Contains<T>(List<T> collection, T element, string message, params string[] parameters)
        {
           Assert.Contains(element, collection);
        }

        /// <inheritdoc/>
        public void DoesNotContain<T>(List<T> collection, T element, string message, params string[] parameters)
        {
            Assert.DoesNotContain(element, collection);
        }

        /// <inheritdoc/>
        public void Matches(string value, Regex pattern, string message, params string[] parameters)
        {
            Assert.Matches(pattern, value);
        }

        /// <inheritdoc/>
        public void StartsWith(string actual, string expected, string message, params string[] parameters)
        {
            Assert.StartsWith(expected, actual);
        }

        /// <inheritdoc/>
        public void EndsWith(string actual, string expected, string message, params string[] parameters)
        {
            Assert.EndsWith(expected, actual);
        }

        /// <inheritdoc/>
        public void Contains(string actual, string expected, string message, params string[] parameters)
        {
            Assert.Contains(expected, actual);
        }

        /// <inheritdoc/>
        public void DoesNotMatch(string value, Regex pattern, string message, params string[] parameters)
        {
            Assert.DoesNotMatch(pattern, value);
        }

        /// <inheritdoc/>
        public void Fail(string message, params string[] parameters)
        {
            Assert.Fail(message);
        }

        /// <inheritdoc/>
        public void IsInstanceOfType(object value, Type expectedType, string message, params string[] parameters)
        {
            Assert.IsType(expectedType, value);
        }

        /// <inheritdoc/>
        public void IsTrue(bool condition, string message, params string[] parameters)
        {
            Assert.True(condition, message);
        }
    }
}

using Models.TestFrameworks.Abstract;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Frameworks.NUnit
{
    /// <summary>
    /// The test framework using NUnit
    /// </summary>
    public class NUnitFramework : ITestFramework
    {
        /// <inheritdoc/>
        public void AreEqual(object expected, object actual, string message, params string[] parameters)
        {
            
            Assert.AreEqual(expected, actual, message, parameters);
        }

        /// <inheritdoc/>
        public void AreNotEqual(object notExpected, object actual, string message, params string[] parameters)
        {
            Assert.AreNotEqual(notExpected, actual, message, parameters);
        }

        /// <inheritdoc/>
        public void Contains<T>(List<T> collection, T element, string message, params string[] parameters)
        {
            CollectionAssert.Contains(collection, element, message, parameters);
        }

        /// <inheritdoc/>
        public void DoesNotContain<T>(List<T> collection, T element, string message, params string[] parameters)
        {
            CollectionAssert.DoesNotContain(collection, element, message, parameters);
        }

        /// <inheritdoc/>
        public void Matches(string value, Regex pattern, string message, params string[] parameters)
        {
            StringAssert.IsMatch(pattern.ToString(), value, message, parameters);
        }

        /// <inheritdoc/>
        public void DoesNotMatch(string value, Regex pattern, string message, params string[] parameters)
        {
            StringAssert.DoesNotMatch(pattern.ToString(), value, message, parameters);
        }

        /// <inheritdoc/>
        public void Fail(string message, params string[] parameters)
        {
            Assert.Fail(message, parameters);
        }

        /// <inheritdoc/>
        public void IsInstanceOfType(object value, Type expectedType, string message, params string[] parameters)
        {
            Assert.IsInstanceOf(expectedType, value, message, parameters);
        }

        /// <inheritdoc/>
        public void IsTrue(bool condition, string message, params string[] parameters)
        {
            Assert.IsTrue(condition, message, parameters);
        }
    }
}

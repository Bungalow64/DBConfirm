using Microsoft.VisualStudio.TestTools.UnitTesting;
using DBConfirm.Core.TestFrameworks.Abstract;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DBConfirm.Frameworks.MSTest
{
    /// <summary>
    /// The test framework using MSTest
    /// </summary>
    public class MSTestFramework : ITestFramework
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
            StringAssert.Matches(value, pattern, message, parameters);
        }

        /// <inheritdoc/>
        public void DoesNotMatch(string value, Regex pattern, string message, params string[] parameters)
        {
            StringAssert.DoesNotMatch(value, pattern, message, parameters);
        }

        /// <inheritdoc/>
        public void Fail(string message, params string[] parameters)
        {
            Assert.Fail(message, parameters);
        }

        /// <inheritdoc/>
        public void IsInstanceOfType(object value, Type expectedType, string message, params string[] parameters)
        {
            Assert.IsInstanceOfType(value, expectedType, message, parameters);
        }

        /// <inheritdoc/>
        public void IsTrue(bool condition, string message, params string[] parameters)
        {
            Assert.IsTrue(condition, message, parameters);
        }
    }
}

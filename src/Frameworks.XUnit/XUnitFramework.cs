using DBConfirm.Core.TestFrameworks.Abstract;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Sdk;

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
            try
            {
                Assert.Equal(expected, actual);
            }
            catch (EqualException ex)
            {
                throw new XUnitException(string.Format(message, parameters), ex);
            }
        }

        /// <inheritdoc/>
        public void AreNotEqual(object notExpected, object actual, string message, params string[] parameters)
        {
            try
            {
                Assert.NotEqual(notExpected, actual);
            }
            catch (NotEqualException ex)
            {
                throw new XUnitException(string.Format(message, parameters), ex);
            }
        }

        /// <inheritdoc/>
        public void Contains<T>(List<T> collection, T element, string message, params string[] parameters)
        {
            try
            {
                Assert.Contains(element, collection);
            }
            catch (ContainsException ex)
            {
                throw new XUnitException(string.Format(message, parameters), ex);
            }
        }

        /// <inheritdoc/>
        public void DoesNotContain<T>(List<T> collection, T element, string message, params string[] parameters)
        {
            try
            {
                Assert.DoesNotContain(element, collection);
            }
            catch (DoesNotContainException ex)
            {
                throw new XUnitException(string.Format(message, parameters), ex);
            }
        }

        /// <inheritdoc/>
        public void Matches(string value, Regex pattern, string message, params string[] parameters)
        {
            try
            {
                Assert.Matches(pattern, value);
            }
            catch (MatchesException ex)
            {
                throw new XUnitException(string.Format(message, parameters), ex);
            }
        }

        /// <inheritdoc/>
        public void StartsWith(string actual, string expected, string message, params string[] parameters)
        {
            try
            {
                Assert.StartsWith(expected, actual);
            }
            catch (StartsWithException ex)
            {
                throw new XUnitException(string.Format(message, parameters), ex);
            }
        }

        /// <inheritdoc/>
        public void EndsWith(string actual, string expected, string message, params string[] parameters)
        {
            try
            {
                Assert.EndsWith(expected, actual);
            }
            catch (EndsWithException ex)
            {
                throw new XUnitException(string.Format(message, parameters), ex);
            }
        }

        /// <inheritdoc/>
        public void Contains(string actual, string expected, string message, params string[] parameters)
        {
            try
            {
                Assert.Contains(expected, actual);
            }
            catch (ContainsException ex)
            {
                throw new XUnitException(string.Format(message, parameters), ex);
            }
        }

        /// <inheritdoc/>
        public void DoesNotMatch(string value, Regex pattern, string message, params string[] parameters)
        {
            try
            {
                Assert.DoesNotMatch(pattern, value);
            }
            catch (DoesNotMatchException ex)
            {
                throw new XUnitException(string.Format(message, parameters), ex);
            }
        }

        /// <inheritdoc/>
        public void Fail(string message, params string[] parameters)
        {
            try
            {
                Assert.Fail(message);
            }
            catch (FailException)
            { 
                throw new XUnitException(string.Format(message, parameters));
            }
        }

        /// <inheritdoc/>
        public void IsInstanceOfType(object value, Type expectedType, string message, params string[] parameters)
        {
            try
            {
                Assert.IsAssignableFrom(expectedType, value);
            }
            catch (IsTypeException ex)
            {
                throw new XUnitException(string.Format(message, parameters), ex);
            }
        }

        /// <inheritdoc/>
        public void IsTrue(bool condition, string message, params string[] parameters)
        {
            try
            {
                Assert.True(condition, message);
            }
            catch (XunitException ex)
            {
                throw new XUnitException(string.Format(message, parameters), ex);
            }
        }
    }
}

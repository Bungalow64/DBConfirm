using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SQLConfirm.Core.TestFrameworks.Abstract
{
    /// <summary>
    /// The interface for a test framework, used to allow framework-dependent assertions and errors to be used
    /// </summary>
    public interface ITestFramework
    {
        /// <summary>
        /// Asserts that two objects are equal, including type
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message to include in the exception when the assertion fails. The message is shown in test results</param>
        /// <param name="parameters">An array of parameters to use when formatting message</param>
        void AreEqual(object expected, object actual, string message, params string[] parameters);

        /// <summary>
        /// Asserts that two objects are not equal, including type
        /// </summary>
        /// <param name="notExpected">The value that is not expected</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message to include in the exception when the assertion fails. The message is shown in test results</param>
        /// <param name="parameters">An array of parameters to use when formatting message</param>
        void AreNotEqual(object notExpected, object actual, string message, params string[] parameters);

        /// <summary>
        /// Asserts that the element exists in the collection
        /// </summary>
        /// <typeparam name="T">The type of object contained within the list</typeparam>
        /// <param name="collection">The collection in which to search for the element</param>
        /// <param name="element">The element that is expected to be found in the collection</param>
        /// <param name="message">The message to include in the exception when the assertion fails. The message is shown in test results</param>
        /// <param name="parameters">An array of parameters to use when formatting message</param>
        void Contains<T>(List<T> collection, T element, string message, params string[] parameters);

        /// <summary>
        /// Asserts that the element does not exist in the collection
        /// </summary>
        /// <typeparam name="T">The type of object contained within the list</typeparam>
        /// <param name="collection">The collection in which to search for the element</param>
        /// <param name="element">The element that is not expected to be found in the collection</param>
        /// <param name="message">The message to include in the exception when the assertion fails. The message is shown in test results</param>
        /// <param name="parameters">An array of parameters to use when formatting message</param>
        void DoesNotContain<T>(List<T> collection, T element, string message, params string[] parameters);

        /// <summary>
        /// Asserts that the condition is true
        /// </summary>
        /// <param name="condition">The condition the test expects to be true</param>
        /// <param name="message">The message to include in the exception when the assertion fails. The message is shown in test results</param>
        /// <param name="parameters">An array of parameters to use when formatting message</param>
        void IsTrue(bool condition, string message, params string[] parameters);

        /// <summary>
        /// Asserts that the object is of the expected type
        /// </summary>
        /// <param name="value">The actual value to test</param>
        /// <param name="expectedType">The expected type</param>
        /// <param name="message">The message to include in the exception when the assertion fails. The message is shown in test results</param>
        /// <param name="parameters">An array of parameters to use when formatting message</param>
        void IsInstanceOfType(object value, Type expectedType, string message, params string[] parameters);

        /// <summary>
        /// Asserts that the value matches the Regex pattern
        /// </summary>
        /// <param name="value">The actual value to test</param>
        /// <param name="pattern">The Regex pattern that is expected to match</param>
        /// <param name="message">The message to include in the exception when the assertion fails. The message is shown in test results</param>
        /// <param name="parameters">An array of parameters to use when formatting message</param>
        void Matches(string value, Regex pattern, string message, params string[] parameters);

        /// <summary>
        /// Asserts that the value does not match the Regex pattern
        /// </summary>
        /// <param name="value">The actual value to test</param>
        /// <param name="pattern">The Regex pattern that is expected not to match</param>
        /// <param name="message">The message to include in the exception when the assertion fails. The message is shown in test results</param>
        /// <param name="parameters">An array of parameters to use when formatting message</param>
        void DoesNotMatch(string value, Regex pattern, string message, params string[] parameters);

        /// <summary>
        /// Triggers an immediate failure of the test
        /// </summary>
        /// <param name="message">The message to include in the exception. The message is shown in test results</param>
        /// <param name="parameters">An array of parameters to use when formatting message</param>
        void Fail(string message, params string[] parameters);
    }
}

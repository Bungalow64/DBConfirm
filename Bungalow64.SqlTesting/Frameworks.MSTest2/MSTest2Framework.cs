using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.TestFrameworks.Abstract;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using FrameworkAssert = Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Frameworks.MSTest2
{
    public class MSTest2Framework : ITestFramework
    {
        public ITestFramework Assert => this;

        public ITestFramework CollectionAssert => this;

        public ITestFramework StringAssert => this;

        public void AreEqual(object expected, object actual, string message, params string[] parameters)
        {
            FrameworkAssert.Assert.AreEqual(expected, actual, message, parameters);
        }

        public void AreNotEqual(object notExpected, object actual, string message, params string[] parameters)
        {
            FrameworkAssert.Assert.AreNotEqual(notExpected, actual, message, parameters);
        }

        public void Contains<T>(List<T> collection, T element, string message, params string[] parameters)
        {
            FrameworkAssert.CollectionAssert.Contains(collection, element, message, parameters);
        }

        public void DoesNotContain<T>(List<T> collection, T element, string message, params string[] parameters)
        {
            FrameworkAssert.CollectionAssert.DoesNotContain(collection, element, message, parameters);
        }

        public void Matches(string value, Regex pattern, string message, params string[] parameters)
        {
            FrameworkAssert.StringAssert.Matches(value, pattern, message, parameters);
        }

        public void DoesNotMatch(string value, Regex pattern, string message, params string[] parameters)
        {
            FrameworkAssert.StringAssert.DoesNotMatch(value, pattern, message, parameters);
        }

        public void Fail(string message, params string[] parameters)
        {
            FrameworkAssert.Assert.Fail(message, parameters);
        }

        public void IsInstanceOfType(object value, Type expectedType, string message, params string[] parameters)
        {
            FrameworkAssert.Assert.IsInstanceOfType(value, expectedType, message, parameters);
        }

        public void IsTrue(bool condition, string message, params string[] parameters)
        {
            FrameworkAssert.Assert.IsTrue(condition, message, parameters);
        }

        public void Error(string message) => throw new AssertFailedException(message);
    }
}

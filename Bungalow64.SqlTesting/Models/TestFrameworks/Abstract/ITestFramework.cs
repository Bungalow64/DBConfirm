using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Models.TestFrameworks.Abstract
{
    public interface ITestFramework
    {
        ITestFramework Assert { get; }
        ITestFramework CollectionAssert { get; }
        ITestFramework StringAssert { get; }

        void AreEqual(object expected, object actual, string message, params string[] parameters);
        void AreNotEqual(object notExpected, object actual, string message, params string[] parameters);
        void Contains<T>(List<T> collection, T element, string message, params string[] parameters);
        void DoesNotContain<T>(List<T> collection, T element, string message, params string[] parameters);
        void IsTrue(bool condition, string message, params string[] parameters);
        void Fail(string message, params string[] parameters);
        void IsInstanceOfType(object value, Type expectedType, string message, params string[] parameters);
        void Matches(string value, Regex pattern, string message, params string[] parameters);
        void DoesNotMatch(string value, Regex pattern, string message, params string[] parameters);
    }
}

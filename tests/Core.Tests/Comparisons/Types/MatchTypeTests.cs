using DBConfirm.Core.Comparisons.Types;
using DBConfirm.Core.TestFrameworks.Abstract;
using NUnit.Framework;
using DBConfirm.Frameworks.MSTest;
using System;

namespace Core.Tests.Comparisons.Types;

[TestFixture]
public class MatchTypeTests
{
    private readonly ITestFramework _testFramework = new MSTestFramework();

    [Test]
    public void MatchType_Ctor_WithType_StoreRegex()
    {
        MatchType matchType = new(typeof(string));

        Assert.AreEqual(typeof(string), matchType.ExpectedType);
    }

    [Test]
    public void MatchType_Ctor_WithNull_ThrowError()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new MatchType((Type)null));

        Assert.AreEqual("Value cannot be null. (Parameter 'expectedType')", exception.Message);
    }

    [TestCase(typeof(int), 1234)]
    [TestCase(typeof(string), "Mike")]
    [TestCase(typeof(double), 152.8d)]
    [TestCase(typeof(bool), true)]
    [TestCase(typeof(float), 152.8f)]
    public void MatchType_Assert_MatchesType_NoError(Type expectedType, object value)
    {
        MatchType matchType = new(expectedType);

        Assert.DoesNotThrow(() => matchType.Assert(_testFramework, value, "Custom message"));
    }

    [Test]
    public void MatchType_Assert_Decimal_MatchesType_NoError()
    {
        MatchType matchType = new(typeof(decimal));

        Assert.DoesNotThrow(() => matchType.Assert(_testFramework, 152m, "Custom message"));
    }

    [TestCase(typeof(string), 1234, "Expected type:<System.String>. Actual type:<System.Int32>")]
    [TestCase(typeof(decimal), 1234, "Expected type:<System.Decimal>. Actual type:<System.Int32>")]
    [TestCase(typeof(double), 1234, "Expected type:<System.Double>. Actual type:<System.Int32>")]
    [TestCase(typeof(float), 1234, "Expected type:<System.Single>. Actual type:<System.Int32>")]
    [TestCase(typeof(int), 1234.5f, "Expected type:<System.Int32>. Actual type:<System.Single>")]
    [TestCase(typeof(int), 1234.5d, "Expected type:<System.Int32>. Actual type:<System.Double>")]
    public void MatchType_Assert_DoesNotMatchType_Error(Type expectedType, object value, string expectedMessage)
    {
        MatchType matchType = new(expectedType);

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() => matchType.Assert(_testFramework, value, "Custom message"));

        Assert.AreEqual($"Assert.IsInstanceOfType failed. Custom message does not match the type {expectedMessage}.", exception.Message);
    }

    [TestCase]
    public void MatchType_Assert_Decimal_DoesNotMatchType_Error()
    {
        MatchType matchType = new(typeof(int));

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() => matchType.Assert(_testFramework, 1234.5m, "Custom message"));

        Assert.AreEqual($"Assert.IsInstanceOfType failed. Custom message does not match the type Expected type:<System.Int32>. Actual type:<System.Decimal>.", exception.Message);
    }

    [TestCase(typeof(int), 1234)]
    [TestCase(typeof(string), "Mike")]
    [TestCase(typeof(double), 152.8d)]
    [TestCase(typeof(bool), true)]
    [TestCase(typeof(float), 152.8f)]
    public void MatchType_Validate_MatchesType_NoError(Type expectedType, object value)
    {
        MatchType matchType = new(expectedType);

        bool result = matchType.Validate(value);

        Assert.AreEqual(true, result);
    }

    [Test]
    public void MatchType_Validate_Decimal_MatchesType_NoError()
    {
        MatchType matchType = new(typeof(decimal));

        bool result = matchType.Validate(152m);

        Assert.AreEqual(true, result);
    }

    [TestCase(typeof(string), 1234)]
    [TestCase(typeof(decimal), 1234)]
    [TestCase(typeof(double), 1234)]
    [TestCase(typeof(float), 1234)]
    [TestCase(typeof(int), 1234.5f)]
    [TestCase(typeof(int), 1234.5d)]
    public void MatchType_Validate_DoesNotMatchType_Error(Type expectedType, object value)
    {
        MatchType matchType = new(expectedType);

        bool result = matchType.Validate(value);

        Assert.AreEqual(false, result);
    }

    [TestCase]
    public void MatchType_Validate_Decimal_DoesNotMatchType_Error()
    {
        MatchType matchType = new(typeof(int));

        bool result = matchType.Validate(1234.5m);

        Assert.AreEqual(false, result);
    }
}

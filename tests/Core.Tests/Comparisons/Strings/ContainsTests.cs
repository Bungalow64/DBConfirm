using DBConfirm.Core.TestFrameworks.Abstract;
using DBConfirm.Frameworks.MSTest;
using NUnit.Framework;
using System;
using Contains = DBConfirm.Core.Comparisons.Strings.Contains;

namespace Core.Tests.Comparisons.Strings;

[TestFixture]
public class ContainsTests
{
    private readonly ITestFramework _testFramework = new MSTestFramework();

    [Test]
    public void Contains_Ctor_SetValue_ValueSet()
    {
        const string value = "ABC";

        Contains contains = new(value);

        Assert.AreEqual(value, contains.Expected);
    }

    [TestCase("")]
    [TestCase(null)]
    public void Contains_Ctor_SetValue_Empty_ThrowError(string value)
    {
        var exception = Assert.Throws<ArgumentException>(() => new Contains(value));

        Assert.AreEqual($"Expected string cannot be null or empty (Parameter 'expected')", exception.Message);
    }

    [TestCase("This is the error message")]
    [TestCase(" is the error")]
    [TestCase("the")]
    [TestCase("e")]
    [TestCase(" ")]
    public void Contains_AssertString_Contains_NoError(string value)
    {
        Contains contains = new(value);

        Assert.DoesNotThrow(() => contains.Assert(_testFramework, "This is the error message", "Custom message"));
    }

    [TestCase("This is the error message2")]
    [TestCase("AThis is the error message")]
    [TestCase("there")]
    [TestCase("THIS IS THE ERROR MESSAGE")]
    [TestCase("E")]
    [TestCase("  ")]
    public void Contains_AssertString_Contains_Error(string value)
    {
        Contains contains = new(value);

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(()
            => contains.Assert(_testFramework, "This is the error message", "Custom message"));

        Assert.AreEqual($"StringAssert.Contains failed. String 'This is the error message' does not contain string '{value}'. Custom message does not contain the expected string.", exception.Message);
    }

    [Test]
    public void Contains_AssertString_Contains_ActualIsNull_Error()
    {
        Contains contains = new("a");

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(()
            => contains.Assert(_testFramework, null, "Custom message"));

        Assert.AreEqual($"Assert.IsInstanceOfType failed. Custom message is not a valid String object", exception.Message);
    }

    [Test]
    public void Contains_AssertString_Contains_ActualIsEmpty_Error()
    {
        Contains contains = new("a");

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(()
            => contains.Assert(_testFramework, "", "Custom message"));

        Assert.AreEqual($"StringAssert.Contains failed. String '' does not contain string 'a'. Custom message does not contain the expected string.", exception.Message);
    }

    [TestCase("This is the error message")]
    [TestCase(" is the error")]
    [TestCase("the")]
    [TestCase("e")]
    [TestCase(" ")]
    public void Contains_ValidateString_Contains_True(string value)
    {
        Contains contains = new(value);

        Assert.AreEqual(true, contains.Validate("This is the error message"));
    }

    [TestCase("This is the error message2")]
    [TestCase("AThis is the error message")]
    [TestCase("there")]
    [TestCase("THIS IS THE ERROR MESSAGE")]
    [TestCase("E")]
    [TestCase("  ")]
    public void Contains_ValidateString_Contains_False(string value)
    {
        Contains contains = new(value);

        Assert.AreEqual(false, contains.Validate("This is the error message"));
    }

    [Test]
    public void Contains_ValidateString_Contains_ActualIsNull_False()
    {
        Contains contains = new("a");

        Assert.AreEqual(false, contains.Validate(null));
    }

    [Test]
    public void Contains_ValidateString_Contains_ActualIsEmpty_True()
    {
        Contains contains = new("a");

        Assert.AreEqual(false, contains.Validate(""));
    }
}

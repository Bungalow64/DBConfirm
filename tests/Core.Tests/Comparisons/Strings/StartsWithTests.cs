using DBConfirm.Core.Comparisons.Strings;
using DBConfirm.Core.TestFrameworks.Abstract;
using DBConfirm.Frameworks.MSTest;
using NUnit.Framework;
using System;

namespace Core.Tests.Comparisons.Strings;

[TestFixture]
public class StartsWithTests
{
    private readonly ITestFramework _testFramework = new MSTestFramework();

    [Test]
    public void StartsWith_Ctor_SetValue_ValueSet()
    {
        const string value = "ABC";

        StartsWith startswith = new(value);

        Assert.AreEqual(value, startswith.Expected);
    }

    [TestCase("")]
    [TestCase(null)]
    public void StartsWith_Ctor_SetValue_Empty_ThrowError(string value)
    {
        var exception = Assert.Throws<ArgumentException>(() => new StartsWith(value));

        Assert.AreEqual($"Expected string cannot be null or empty (Parameter 'expected')", exception.Message);
    }

    [TestCase("This is the error message")]
    [TestCase("This is the")]
    [TestCase("This is ")]
    [TestCase("This ")]
    [TestCase("This")]
    [TestCase("T")]
    public void StartsWith_AssertString_StartsWith_NoError(string value)
    {
        StartsWith startswith = new(value);

        Assert.DoesNotThrow(() => startswith.Assert(_testFramework, "This is the error message", "Custom message"));
    }

    [TestCase("This is the error message2")]
    [TestCase("AThis is the error message")]
    [TestCase("there")]
    [TestCase("THIS IS THE ERROR MESSAGE")]
    [TestCase("E")]
    [TestCase("t")]
    [TestCase(" ")]
    public void StartsWith_AssertString_StartsWith_False(string value)
    {
        StartsWith startswith = new(value);

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(()
            => startswith.Assert(_testFramework, "This is the error message", "Custom message"));

        Assert.AreEqual($"StringAssert.StartsWith failed. String 'This is the error message' does not start with string '{value}'. Custom message does not start with the expected string.", exception.Message);
    }

    [Test]
    public void StartsWith_AssertString_StartsWith_ActualIsNull_False()
    {
        StartsWith startswith = new("a");

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(()
            => startswith.Assert(_testFramework, null, "Custom message"));

        Assert.AreEqual($"Assert.IsInstanceOfType failed. Custom message is not a valid String object", exception.Message);
    }

    [Test]
    public void StartsWith_AssertString_StartsWith_ActualIsEmpty_False()
    {
        StartsWith startswith = new("a");

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(()
            => startswith.Assert(_testFramework, "", "Custom message"));

        Assert.AreEqual($"StringAssert.StartsWith failed. String '' does not start with string 'a'. Custom message does not start with the expected string.", exception.Message);
    }

    [TestCase("This is the error message")]
    [TestCase("This is the")]
    [TestCase("This is ")]
    [TestCase("This ")]
    [TestCase("This")]
    [TestCase("T")]
    public void StartsWith_ValidateString_StartsWith_True(string value)
    {
        StartsWith startswith = new(value);

        Assert.AreEqual(true, startswith.Validate("This is the error message"));
    }

    [TestCase("This is the error message2")]
    [TestCase("AThis is the error message")]
    [TestCase("there")]
    [TestCase("THIS IS THE ERROR MESSAGE")]
    [TestCase("E")]
    [TestCase("t")]
    [TestCase(" ")]
    public void StartsWith_ValidateString_StartsWith_False(string value)
    {
        StartsWith startswith = new(value);

        Assert.AreEqual(false, startswith.Validate("This is the error message"));
    }

    [Test]
    public void StartsWith_ValidateString_StartsWith_ActualIsNull_False()
    {
        StartsWith startswith = new("a");

        Assert.AreEqual(false, startswith.Validate(null));
    }

    [Test]
    public void StartsWith_ValidateString_StartsWith_ActualIsEmpty_False()
    {
        StartsWith startswith = new("a");

        Assert.AreEqual(false, startswith.Validate(""));
    }
}

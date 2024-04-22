using DBConfirm.Core.Comparisons.Strings;
using DBConfirm.Core.TestFrameworks.Abstract;
using DBConfirm.Frameworks.MSTest;
using NUnit.Framework;
using System;
using System.ComponentModel;

namespace Core.Tests.Comparisons.Strings;

[TestFixture]
public class EndsWithTests
{
    private readonly ITestFramework _testFramework = new MSTestFramework();

    [Test]
    public void EndsWith_Ctor_SetValue_ValueSet()
    {
        const string value = "ABC";

        EndsWith endswith = new(value);

        Assert.AreEqual(value, endswith.Expected);
    }

    [TestCase("")]
    [TestCase(null)]
    public void EndsWith_Ctor_SetValue_Empty_ThrowError(string value)
    {
        var exception = Assert.Throws<ArgumentException>(() => new EndsWith(value));

        Assert.AreEqual($"Expected string cannot be null or empty (Parameter 'expected')", exception.Message);
    }

    [TestCase("This is the error message")]
    [TestCase(" is the error message")]
    [TestCase("is the error message")]
    [TestCase("error message")]
    [TestCase("r message")]
    [TestCase(" message")]
    [TestCase("message")]
    [TestCase("e")]
    public void EndsWith_AssertString_EndsWith_NoError(string value)
    {
        EndsWith endswith = new(value);

        Assert.DoesNotThrow(() => endswith.Assert(_testFramework, "This is the error message", "Custom message"));
    }

    [TestCase("2This is the error message")]
    [TestCase("This is the error messageA")]
    [TestCase("there")]
    [TestCase("THIS IS THE ERROR MESSAGE")]
    [TestCase("T")]
    [TestCase("E")]
    [TestCase(" ")]
    public void EndsWith_AssertString_EndsWith_Error(string value)
    {
        EndsWith endswith = new(value);

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(()
            => endswith.Assert(_testFramework, "This is the error message", "Custom message"));

        Assert.AreEqual($"StringAssert.EndsWith failed. String 'This is the error message' does not end with string '{value}'. Custom message does not end with the expected string.", exception.Message);
    }

    [Test]
    public void EndsWith_AssertString_EndsWith_ActualIsNull_Error()
    {
        EndsWith endswith = new("a");

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(()
            => endswith.Assert(_testFramework, null, "Custom message"));

        Assert.AreEqual($"Assert.IsInstanceOfType failed. Custom message is not a valid String object", exception.Message);
    }

    [Test]
    public void EndsWith_AssertString_EndsWith_ActualIsEmpty_Error()
    {
        EndsWith endswith = new("a");

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(()
            => endswith.Assert(_testFramework, "", "Custom message"));

        Assert.AreEqual($"StringAssert.EndsWith failed. String '' does not end with string 'a'. Custom message does not end with the expected string.", exception.Message);
    }

    [TestCase("This is the error message")]
    [TestCase(" is the error message")]
    [TestCase("is the error message")]
    [TestCase("error message")]
    [TestCase("r message")]
    [TestCase(" message")]
    [TestCase("message")]
    [TestCase("e")]
    public void EndsWith_ValidateString_EndsWith_True(string value)
    {
        EndsWith endswith = new(value);

        Assert.AreEqual(true, endswith.Validate("This is the error message"));
    }

    [TestCase("2This is the error message")]
    [TestCase("This is the error messageA")]
    [TestCase("there")]
    [TestCase("THIS IS THE ERROR MESSAGE")]
    [TestCase("T")]
    [TestCase("E")]
    [TestCase(" ")]
    public void EndsWith_ValidateString_EndsWith_False(string value)
    {
        EndsWith endswith = new(value);

        Assert.AreEqual(false, endswith.Validate("This is the error message"));
    }

    [Test]
    public void EndsWith_ValidateString_EndsWith_ActualIsNull_False()
    {
        EndsWith endswith = new("a");

        Assert.AreEqual(false, endswith.Validate(null));
    }

    [Test]
    public void EndsWith_ValidateString_EndsWith_ActualIsEmpty_False()
    {
        EndsWith endswith = new("a");

        Assert.AreEqual(false, endswith.Validate(""));
    }
}

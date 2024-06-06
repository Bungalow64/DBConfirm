using DBConfirm.Core.DataResults;
using DBConfirm.Core.TestFrameworks.Abstract;
using NUnit.Framework;
using DBConfirm.Frameworks.MSTest;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Core.Tests.DataResults;

[TestFixture]
public class ErrorResultTests
{
    private readonly ITestFramework _testFramework = new MSTestFramework();

    [Test]
    public void ErrorResult_Ctor_NullValue_ValueSet()
    {
        ErrorResult result = new(_testFramework, null);

        Assert.AreEqual(null, result.RawData);
    }

    [Test]
    public void ErrorResult_Ctor_WithException_ValueSet()
    {
        var error = new NullReferenceException();

        ErrorResult result = new(_testFramework, error);

        Assert.AreEqual(error, result.RawData);
    }

    [Test]
    public void ErrorResult_AssertType_ValueMatches_NoError()
    {
        var error = new NullReferenceException();

        ErrorResult result = new(_testFramework, error);

        Assert.DoesNotThrow(() =>
            result.AssertType(typeof(NullReferenceException)));
    }

    [Test]
    public void ErrorResult_AssertType_ValueDoesNotMatch_Error()
    {
        var error = new NullReferenceException();

        ErrorResult result = new(_testFramework, error);

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
           result.AssertType(typeof(InsufficientMemoryException)));

        Assert.AreEqual("Assert.AreEqual failed. Expected:<System.InsufficientMemoryException>. Actual:<System.NullReferenceException>. Error result has an unexpected value", exception.Message);
    }

    [Test]
    public void ErrorResult_AssertType_ValueDoesNotMatchExpectedIsNull_Error()
    {
        var error = new NullReferenceException();

        ErrorResult result = new(_testFramework, error);

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
           result.AssertType(null));

        Assert.AreEqual("Assert.AreEqual failed. Expected:<(null)>. Actual:<System.NullReferenceException>. Error result has an unexpected value", exception.Message);
    }
}

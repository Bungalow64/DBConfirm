using DBConfirm.Core.Comparisons.States;
using DBConfirm.Core.TestFrameworks.Abstract;
using NUnit.Framework;
using DBConfirm.Frameworks.MSTest;
using System;

namespace Core.Tests.Comparisons.States;

[TestFixture]
public class NullStateTests
{
    private readonly ITestFramework _testFramework = new MSTestFramework();

    [Test]
    public void NullState_HasValue_Error()
    {
        object value = 123;

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() => new NullState()
            .Assert(_testFramework, value, "CustomMessage"));

        Assert.AreEqual("Assert.AreEqual failed. Expected:< (System.DBNull)>. Actual:<123 (System.Int32)>. CustomMessage has an unexpected state", exception.Message);
    }

    [Test]
    public void NullState_HasNullValue_NoError()
    {
        object value = null;

        new NullState()
            .Assert(_testFramework, value, "CustomMessage");
    }

    [Test]
    public void NullState_HasDBNullValue_NoError()
    {
        object value = DBNull.Value;

        new NullState()
            .Assert(_testFramework, value, "CustomMessage");
    }
}

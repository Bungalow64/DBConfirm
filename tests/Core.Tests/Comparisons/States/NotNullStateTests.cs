using DBConfirm.Core.Comparisons.States;
using DBConfirm.Core.TestFrameworks.Abstract;
using NUnit.Framework;
using DBConfirm.Frameworks.MSTest;
using System;

namespace Core.Tests.Comparisons.States;

[TestFixture]
public class NotNullStateTests
{
    private readonly ITestFramework _testFramework = new MSTestFramework();

    [Test]
    public void NotNullState_HasValue_NoError()
    {
        object value = 123;

        new NotNullState()
            .Assert(_testFramework, value, "CustomMessage");
    }

    [Test]
    public void NotNullState_HasNullValue_Error()
    {
        object value = null;

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() => new NotNullState()
            .Assert(_testFramework, value, "CustomMessage"));

        Assert.AreEqual("Assert.AreNotEqual failed. Expected any value except:<>. Actual:<>. CustomMessage has an unexpected state", exception.Message);
    }

    [Test]
    public void NotNullState_HasDBNullValue_Error()
    {
        object value = DBNull.Value;

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() => new NotNullState()
            .Assert(_testFramework, value, "CustomMessage"));

        Assert.AreEqual("Assert.AreNotEqual failed. Expected any value except:<>. Actual:<>. CustomMessage has an unexpected state", exception.Message);
    }
}

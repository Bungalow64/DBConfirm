﻿using DBConfirm.Core.Comparisons.Strings;
using DBConfirm.Core.TestFrameworks.Abstract;
using NUnit.Framework;
using DBConfirm.Frameworks.MSTest;
using System;
using System.Text.RegularExpressions;

namespace Core.Tests.Comparisons.Strings;

[TestFixture]
public class NoMatchRegexTests
{
    private readonly ITestFramework _testFramework = new MSTestFramework();

    [Test]
    public void NoMatchRegex_Ctor_WithRegex_StoreRegex()
    {
        Regex regex = new(@"\b[M]\w+");
        NoMatchRegex matchRegex = new(regex);

        Assert.AreEqual(regex, matchRegex.UnexpectedRegex);
    }

    [Test]
    public void NoMatchRegex_Ctor_WithString_StoreRegex()
    {
        NoMatchRegex matchRegex = new(@"\b[M]\w+");

        Assert.AreEqual(@"\b[M]\w+", matchRegex.UnexpectedRegex.ToString());
    }

    [Test]
    public void NoMatchRegex_Ctor_WithNullString_ThrowError()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new NoMatchRegex((string)null));

        Assert.AreEqual("Value cannot be null. (Parameter 'unexpectedRegex')", exception.Message);
    }

    [Test]
    public void NoMatchRegex_Ctor_WithNullRegex_ThrowError()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new NoMatchRegex((string)null));

        Assert.AreEqual("Value cannot be null. (Parameter 'unexpectedRegex')", exception.Message);
    }

    [Test]
    public void NoMatchRegex_AssertString_DoesNotMatchRegex_NoError()
    {
        NoMatchRegex matchRegex = new(@"\b[M]\w+");

        Assert.DoesNotThrow(() => matchRegex.Assert(_testFramework, "Brian", "Custom message"));
    }

    [Test]
    public void NoMatchRegex_AssertString_MatchesRegex_Error()
    {
        NoMatchRegex matchRegex = new(@"\b[M]\w+");

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() => matchRegex.Assert(_testFramework, "Mike", "Custom message"));

        Assert.AreEqual("StringAssert.DoesNotMatch failed. String 'Mike' matches pattern '\\b[M]\\w+'. Custom message matches the regex when it should not match.", exception.Message);
    }
}

using Models.Strings;
using Models.TestFrameworks.Abstract;
using NUnit.Framework;
using System;
using System.Text.RegularExpressions;

namespace Models.Tests.Strings
{
    [TestFixture]
    public class MatchRegexTests
    {
        private readonly ITestFramework _testFramework = new Frameworks.MSTest2.MSTest2Framework();

        [Test]
        public void MatchRegex_Ctor_WithRegex_StoreRegex()
        {
            Regex regex = new Regex(@"\b[M]\w+");
            MatchRegex matchRegex = new MatchRegex(regex);

            Assert.AreEqual(regex, matchRegex.ExpectedRegex);
        }

        [Test]
        public void MatchRegex_Ctor_WithString_StoreRegex()
        {
            MatchRegex matchRegex = new MatchRegex(@"\b[M]\w+");

            Assert.AreEqual(@"\b[M]\w+", matchRegex.ExpectedRegex.ToString());
        }

        [Test]
        public void MatchRegex_Ctor_WithNullString_ThrowError()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new MatchRegex((string)null));

            Assert.AreEqual("Value cannot be null. (Parameter 'expectedRegex')", exception.Message);
        }

        [Test]
        public void MatchRegex_Ctor_WithNullRegex_ThrowError()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new MatchRegex((string)null));

            Assert.AreEqual("Value cannot be null. (Parameter 'expectedRegex')", exception.Message);
        }

        [Test]
        public void MatchRegex_AssertString_MatchesRegex_NoError()
        {
            MatchRegex matchRegex = new MatchRegex(@"\b[M]\w+");

            Assert.DoesNotThrow(() => matchRegex.AssertString(_testFramework, "Mike", "Custom message: {0}"));
        }

        [Test]
        public void MatchRegex_AssertString_DoesNotMatchRegex_Error()
        {
            MatchRegex matchRegex = new MatchRegex(@"\b[M]\w+");

            var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() => matchRegex.AssertString(_testFramework, "Brian", "Custom message: {0}"));

            Assert.AreEqual("StringAssert.Matches failed. String 'Brian' does not match pattern '\\b[M]\\w+'. Custom message: does not match the regex.", exception.Message);
        }
    }
}

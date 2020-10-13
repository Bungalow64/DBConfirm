using Models.Strings;
using NUnit.Framework;
using System;
using System.Text.RegularExpressions;

namespace Models.Tests.Strings
{
    [TestFixture]
    public class NoMatchRegexTests
    {
        [Test]
        public void NoMatchRegex_Ctor_WithRegex_StoreRegex()
        {
            Regex regex = new Regex(@"\b[M]\w+");
            NoMatchRegex matchRegex = new NoMatchRegex(regex);

            Assert.AreEqual(regex, matchRegex.UnexpectedRegex);
        }

        [Test]
        public void NoMatchRegex_Ctor_WithString_StoreRegex()
        {
            NoMatchRegex matchRegex = new NoMatchRegex(@"\b[M]\w+");

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
            NoMatchRegex matchRegex = new NoMatchRegex(@"\b[M]\w+");

            Assert.DoesNotThrow(() => matchRegex.AssertString("Brian", "Custom message: {0}"));
        }

        [Test]
        public void NoMatchRegex_AssertString_MatchesRegex_Error()
        {
            NoMatchRegex matchRegex = new NoMatchRegex(@"\b[M]\w+");

            var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() => matchRegex.AssertString("Mike", "Custom message: {0}"));

            Assert.AreEqual("StringAssert.DoesNotMatch failed. String 'Mike' matches pattern '\\b[M]\\w+'. Custom message: matches the regex when it should not match.", exception.Message);
        }
    }
}

using DBConfirm.Core.Comparisons.Strings;
using DBConfirm.Core.TestFrameworks.Abstract;
using NUnit.Framework;
using DBConfirm.Frameworks.MSTest;
using System;

namespace SqlConfirm.Core.Tests.Strings
{
    [TestFixture]
    public class SpecificLengthTests
    {
        private readonly ITestFramework _testFramework = new MSTestFramework();

        [Test]
        public void SpecificLength_Ctor_SetExpectedLength_LengthSet()
        {
            const int length = 123;

            SpecificLength specificLength = new SpecificLength(length);

            Assert.AreEqual(length, specificLength.ExpectedLength);
        }

        [Test]
        public void SpecificLength_Ctor_SetNegativeLength_Error()
        {
            const int length = -1;

            var exception = Assert.Throws<ArgumentException>(() => new SpecificLength(length));

            Assert.AreEqual($"Expected length cannot be less than 0 (Parameter 'expectedLength')", exception.Message);
        }

        [TestCase(0, "")]
        [TestCase(0, null)]
        [TestCase(1, "A")]
        [TestCase(3, "ABC")]
        public void SpecificLength_AssertString_LengthMatches_NoError(int length, string value)
        {
            SpecificLength specificLength = new SpecificLength(length);

            Assert.DoesNotThrow(() => specificLength.Assert(_testFramework, value, "Custom message"));
        }

        [TestCase(1, "")]
        [TestCase(1, null)]
        [TestCase(2, "A")]
        [TestCase(0, "ABC")]
        [TestCase(1, "ABC")]
        public void SpecificLength_AssertString_LengthDoesNotMatch_Error(int length, string value)
        {
            SpecificLength specificLength = new SpecificLength(length);

            var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() 
                => specificLength.Assert(_testFramework, value, "Custom message"));

            Assert.AreEqual($"Assert.AreEqual failed. Expected:<{length}>. Actual:<{value?.Length ?? 0}>. Custom message has an unexpected length", exception.Message);
        }
    }
}

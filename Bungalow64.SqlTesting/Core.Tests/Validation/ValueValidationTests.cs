using SQLConfirm.Core.Comparisons.Abstract;
using SQLConfirm.Core.Comparisons.Dates.Abstract;
using SQLConfirm.Core.TestFrameworks.Abstract;
using SQLConfirm.Core.Validation;
using Moq;
using NUnit.Framework;
using SQLConfirm.Frameworks.MSTest2;
using System;

namespace SqlConfirm.Core.Tests.Validation
{
    [TestFixture]
    public class ValueValidationTests
    {
        private readonly ITestFramework _testFramework = new MSTest2Framework();

        [TestCase(123, 123)]
        [TestCase(123.5, 123.5)]
        [TestCase("123", "123")]
        [TestCase(true, true)]
        [TestCase(null, null)]
        public void ValueValidation_Assert_BasicObjects_AreEqual(object expectedValue, object actualValue)
        {
            Assert.DoesNotThrow(() => ValueValidation.Assert(_testFramework, expectedValue, actualValue, "Custom assertion"));
        }

        [TestCase(123, 124)]
        [TestCase(123.5, 123.6)]
        [TestCase("123", "124")]
        [TestCase(true, false)]
        public void ValueValidation_Assert_BasicObjects_AreNotEqual(object expectedValue, object actualValue)
        {
            var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() => 
                ValueValidation.Assert(_testFramework, expectedValue, actualValue, "Custom assertion"));

            Assert.AreEqual($"Assert.AreEqual failed. Expected:<{ expectedValue }>. Actual:<{ actualValue }>. Custom assertion has an unexpected value", exception.Message);
        }

        [Test]
        public void ValueValidation_Assert_ExpectNullActualDBNull_AreEqual()
        {
            Assert.DoesNotThrow(() => ValueValidation.Assert(_testFramework, null, DBNull.Value, "Custom assertion"));
        }

        [Test]
        public void ValueValidation_Assert_ExpectDBNullActualNull_AreEqual()
        {
            Assert.DoesNotThrow(() => ValueValidation.Assert(_testFramework, DBNull.Value, null, "Custom assertion"));
        }

        [Test]
        public void ValueValidation_Assert_ExpectDBNull_AreEqual()
        {
            Assert.DoesNotThrow(() => ValueValidation.Assert(_testFramework, DBNull.Value, DBNull.Value, "Custom assertion"));
        }

        [Test]
        public void ValueValidation_Assert_ExpectStringActualInt_AreNotEqual()
        {
            var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() => 
                ValueValidation.Assert(_testFramework, "123", 123, "Custom assertion"));

            Assert.AreEqual($"Assert.AreEqual failed. Expected:<123 (System.String)>. Actual:<123 (System.Int32)>. Custom assertion has an unexpected value", exception.Message);
        }

        [Test]
        public void ValueValidation_Assert_AssertState_AreNotEqual_ThrowError()
        {
            object requestedValue = null;
            string requestedMessage = null;

            Mock<IComparison> state = new Mock<IComparison>(MockBehavior.Strict);

            state
                .Setup(p => p.Assert(It.IsAny<ITestFramework>(), It.IsAny<object>(), It.IsAny<string>()))
                .Callback<ITestFramework, object, string>((s, p, q) =>
                {
                    requestedValue = p;
                    requestedMessage = q;
                })
                .Throws(new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException());

            Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() => 
                ValueValidation.Assert(_testFramework, state.Object, "ValueA", "Custom assertion"));

            state
                .Verify(p => p.Assert(It.IsAny<ITestFramework>(), It.IsAny<object>(), It.IsAny<string>()), Times.Once);

            Assert.AreEqual("ValueA", requestedValue);
            Assert.AreEqual("Custom assertion", requestedMessage);
        }

        [Test]
        public void ValueValidation_Assert_AssertState_AreEqual_ThrowNoError()
        {
            object requestedValue = null;
            string requestedMessage = null;

            Mock<IComparison> state = new Mock<IComparison>(MockBehavior.Strict);

            state
                .Setup(p => p.Assert(It.IsAny<ITestFramework>(), It.IsAny<object>(), It.IsAny<string>()))
                .Callback<ITestFramework, object, string>((s, p, q) =>
                {
                    requestedValue = p;
                    requestedMessage = q;
                });

            Assert.DoesNotThrow(() => 
                ValueValidation.Assert(_testFramework, state.Object, "ValueA", "Custom assertion"));

            state
                .Verify(p => p.Assert(It.IsAny<ITestFramework>(), It.IsAny<object>(), It.IsAny<string>()), Times.Once);

            Assert.AreEqual("ValueA", requestedValue);
            Assert.AreEqual("Custom assertion", requestedMessage);
        }

        [Test]
        public void ValueValidation_Assert_AssertDate_AreNotEqual_ThrowError()
        {
            object requestedValue = null;
            string requestedMessage = null;

            Mock<IDateComparison> state = new Mock<IDateComparison>(MockBehavior.Strict);

            state
                .Setup(p => p.Assert(It.IsAny<ITestFramework>(), It.IsAny<object>(), It.IsAny<string>()))
                .Callback<ITestFramework, object, string>((s, p, q) =>
                {
                    requestedValue = p;
                    requestedMessage = q;
                })
                .Throws(new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException());

            Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
                ValueValidation.Assert(_testFramework, state.Object, DateTime.Parse("01-Mar-2020"), "Custom assertion"));

            state
                .Verify(p => p.Assert(It.IsAny<ITestFramework>(), It.IsAny<DateTime>(), It.IsAny<string>()), Times.Once);

            Assert.AreEqual(DateTime.Parse("01-Mar-2020"), requestedValue);
            Assert.AreEqual("Custom assertion", requestedMessage);
        }

        [Test]
        public void ValueValidation_Assert_AssertDate_AreEqual_ThrowNoError()
        {
            object requestedValue = null;
            string requestedMessage = null;

            Mock<IDateComparison> state = new Mock<IDateComparison>(MockBehavior.Strict);

            state
                .Setup(p => p.Assert(It.IsAny<ITestFramework>(), It.IsAny<object>(), It.IsAny<string>()))
                .Callback<ITestFramework, object, string>((s, p, q) =>
                {
                    requestedValue = p;
                    requestedMessage = q;
                });

            Assert.DoesNotThrow(() =>
                ValueValidation.Assert(_testFramework, state.Object, DateTime.Parse("01-Mar-2020"), "Custom assertion"));

            state
                .Verify(p => p.Assert(It.IsAny<ITestFramework>(), It.IsAny<DateTime>(), It.IsAny<string>()), Times.Once);

            Assert.AreEqual(DateTime.Parse("01-Mar-2020"), requestedValue);
            Assert.AreEqual("Custom assertion", requestedMessage);
        }

        [Test]
        public void ValueValidation_Assert_AssertDate_ActualValueNotADate_ThrowError()
        {
            object requestedValue = null;
            string requestedMessage = null;

            Mock<IDateComparison> state = new Mock<IDateComparison>(MockBehavior.Strict);

            state
                .Setup(p => p.Assert(It.IsAny<ITestFramework>(), It.IsAny<object>(), It.IsAny<string>()))
                .Callback<ITestFramework, object, string>((s, p, q) =>
                {
                    requestedValue = p;
                    requestedMessage = q;
                })
                .Throws(new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException());

            Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
                ValueValidation.Assert(_testFramework, state.Object, 123, "Custom assertion"));

            state
                .Verify(p => p.Assert(It.IsAny<ITestFramework>(), It.IsAny<object>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void ValueValidation_Assert_AssertString_AreNotEqual_ThrowError()
        {
            object requestedValue = null;
            string requestedMessage = null;

            Mock<IComparison> state = new Mock<IComparison>(MockBehavior.Strict);

            state
                .Setup(p => p.Assert(It.IsAny<ITestFramework>(), It.IsAny<object>(), It.IsAny<string>()))
                .Callback<ITestFramework, object, string>((s, p, q) =>
                {
                    requestedValue = p;
                    requestedMessage = q;
                })
                .Throws(new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException());

            Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
                ValueValidation.Assert(_testFramework, state.Object, "ABC", "Custom assertion"));

            state
                .Verify(p => p.Assert(It.IsAny<ITestFramework>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            Assert.AreEqual("ABC", requestedValue);
            Assert.AreEqual("Custom assertion", requestedMessage);
        }

        [Test]
        public void ValueValidation_Assert_AssertString_AreEqual_ThrowNoError()
        {
            object requestedValue = null;
            string requestedMessage = null;

            Mock<IComparison> state = new Mock<IComparison>(MockBehavior.Strict);

            state
                .Setup(p => p.Assert(It.IsAny<ITestFramework>(), It.IsAny<object>(), It.IsAny<string>()))
                .Callback<ITestFramework, object, string>((s, p, q) =>
                {
                    requestedValue = p;
                    requestedMessage = q;
                });

            Assert.DoesNotThrow(() =>
                ValueValidation.Assert(_testFramework, state.Object, "ABC", "Custom assertion"));

            state
                .Verify(p => p.Assert(It.IsAny<ITestFramework>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            Assert.AreEqual("ABC", requestedValue);
            Assert.AreEqual("Custom assertion", requestedMessage);
        }

        [Test]
        public void ValueValidation_Assert_AssertString_ActualValueNotAString_ThrowError()
        {
            object requestedValue = null;
            string requestedMessage = null;

            Mock<IComparison> state = new Mock<IComparison>(MockBehavior.Strict);

            state
                .Setup(p => p.Assert(It.IsAny<ITestFramework>(), It.IsAny<object>(), It.IsAny<string>()))
                .Callback<ITestFramework, object, string>((s, p, q) =>
                {
                    requestedValue = p;
                    requestedMessage = q;
                })
                .Throws(new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException());

            Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
                ValueValidation.Assert(_testFramework, state.Object, 123, "Custom assertion"));

            state
                .Verify(p => p.Assert(It.IsAny<ITestFramework>(), It.IsAny<object>(), It.IsAny<string>()), Times.Once);
        }
    }
}

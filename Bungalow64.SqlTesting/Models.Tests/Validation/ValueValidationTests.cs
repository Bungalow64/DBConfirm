using Models.Dates.Abstract;
using Models.States.Abstract;
using Models.Strings.Abstract;
using Models.TestFrameworks.Abstract;
using Models.Validation;
using Moq;
using NUnit.Framework;
using System;

namespace Models.Tests.Validation
{
    [TestFixture]
    public class ValueValidationTests
    {
        private readonly ITestFramework _testFramework = new Frameworks.MSTest2.MSTest2Framework();

        [TestCase(123, 123)]
        [TestCase(123.5, 123.5)]
        [TestCase("123", "123")]
        [TestCase(true, true)]
        [TestCase(null, null)]
        public void ValueValidation_Validate_BasicObjects_AreEqual(object expectedValue, object actualValue)
        {
            Assert.DoesNotThrow(() => ValueValidation.Validate(_testFramework, expectedValue, actualValue, "Custom assertion"));
        }

        [TestCase(123, 124)]
        [TestCase(123.5, 123.6)]
        [TestCase("123", "124")]
        [TestCase(true, false)]
        public void ValueValidation_Validate_BasicObjects_AreNotEqual(object expectedValue, object actualValue)
        {
            var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() => 
                ValueValidation.Validate(_testFramework, expectedValue, actualValue, "Custom assertion"));

            Assert.AreEqual($"Assert.AreEqual failed. Expected:<{ expectedValue }>. Actual:<{ actualValue }>. Custom assertion has an unexpected value", exception.Message);
        }

        [Test]
        public void ValueValidation_Validate_ExpectNullActualDBNull_AreEqual()
        {
            Assert.DoesNotThrow(() => ValueValidation.Validate(_testFramework, null, DBNull.Value, "Custom assertion"));
        }

        [Test]
        public void ValueValidation_Validate_ExpectDBNullActualNull_AreEqual()
        {
            Assert.DoesNotThrow(() => ValueValidation.Validate(_testFramework, DBNull.Value, null, "Custom assertion"));
        }

        [Test]
        public void ValueValidation_Validate_ExpectDBNull_AreEqual()
        {
            Assert.DoesNotThrow(() => ValueValidation.Validate(_testFramework, DBNull.Value, DBNull.Value, "Custom assertion"));
        }

        [Test]
        public void ValueValidation_Validate_ExpectStringActualInt_AreNotEqual()
        {
            var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() => 
                ValueValidation.Validate(_testFramework, "123", 123, "Custom assertion"));

            Assert.AreEqual($"Assert.AreEqual failed. Expected:<123 (System.String)>. Actual:<123 (System.Int32)>. Custom assertion has an unexpected value", exception.Message);
        }

        [Test]
        public void ValueValidation_Validate_AssertState_AreNotEqual_ThrowError()
        {
            object requestedValue = null;
            string requestedMessage = null;

            Mock<IState> state = new Mock<IState>(MockBehavior.Strict);

            state
                .Setup(p => p.AssertState(It.IsAny<ITestFramework>(), It.IsAny<object>(), It.IsAny<string>()))
                .Callback<ITestFramework, object, string>((s, p, q) =>
                {
                    requestedValue = p;
                    requestedMessage = q;
                })
                .Throws(new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException());

            Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() => 
                ValueValidation.Validate(_testFramework, state.Object, "ValueA", "Custom assertion"));

            state
                .Verify(p => p.AssertState(It.IsAny<ITestFramework>(), It.IsAny<object>(), It.IsAny<string>()), Times.Once);

            Assert.AreEqual("ValueA", requestedValue);
            Assert.AreEqual("Custom assertion has an unexpected state", requestedMessage);
        }

        [Test]
        public void ValueValidation_Validate_AssertState_AreEqual_ThrowNoError()
        {
            object requestedValue = null;
            string requestedMessage = null;

            Mock<IState> state = new Mock<IState>(MockBehavior.Strict);

            state
                .Setup(p => p.AssertState(It.IsAny<ITestFramework>(), It.IsAny<object>(), It.IsAny<string>()))
                .Callback<ITestFramework, object, string>((s, p, q) =>
                {
                    requestedValue = p;
                    requestedMessage = q;
                });

            Assert.DoesNotThrow(() => 
                ValueValidation.Validate(_testFramework, state.Object, "ValueA", "Custom assertion"));

            state
                .Verify(p => p.AssertState(It.IsAny<ITestFramework>(), It.IsAny<object>(), It.IsAny<string>()), Times.Once);

            Assert.AreEqual("ValueA", requestedValue);
            Assert.AreEqual("Custom assertion has an unexpected state", requestedMessage);
        }

        [Test]
        public void ValueValidation_Validate_AssertDate_AreNotEqual_ThrowError()
        {
            DateTime? requestedValue = null;
            string requestedMessage = null;

            Mock<IDateComparison> state = new Mock<IDateComparison>(MockBehavior.Strict);

            state
                .Setup(p => p.AssertDate(It.IsAny<ITestFramework>(), It.IsAny<DateTime>(), It.IsAny<string>()))
                .Callback<ITestFramework, DateTime, string>((s, p, q) =>
                {
                    requestedValue = p;
                    requestedMessage = q;
                })
                .Throws(new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException());

            Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
                ValueValidation.Validate(_testFramework, state.Object, DateTime.Parse("01-Mar-2020"), "Custom assertion"));

            state
                .Verify(p => p.AssertDate(It.IsAny<ITestFramework>(), It.IsAny<DateTime>(), It.IsAny<string>()), Times.Once);

            Assert.AreEqual(DateTime.Parse("01-Mar-2020"), requestedValue);
            Assert.AreEqual("Custom assertion is different by {0}", requestedMessage);
        }

        [Test]
        public void ValueValidation_Validate_AssertDate_AreEqual_ThrowNoError()
        {
            DateTime? requestedValue = null;
            string requestedMessage = null;

            Mock<IDateComparison> state = new Mock<IDateComparison>(MockBehavior.Strict);

            state
                .Setup(p => p.AssertDate(It.IsAny<ITestFramework>(), It.IsAny<DateTime>(), It.IsAny<string>()))
                .Callback<ITestFramework, DateTime, string>((s, p, q) =>
                {
                    requestedValue = p;
                    requestedMessage = q;
                });

            Assert.DoesNotThrow(() =>
                ValueValidation.Validate(_testFramework, state.Object, DateTime.Parse("01-Mar-2020"), "Custom assertion"));

            state
                .Verify(p => p.AssertDate(It.IsAny<ITestFramework>(), It.IsAny<DateTime>(), It.IsAny<string>()), Times.Once);

            Assert.AreEqual(DateTime.Parse("01-Mar-2020"), requestedValue);
            Assert.AreEqual("Custom assertion is different by {0}", requestedMessage);
        }

        [Test]
        public void ValueValidation_Validate_AssertDate_ActualValueNotADate_ThrowError()
        {
            DateTime? requestedValue = null;
            string requestedMessage = null;

            Mock<IDateComparison> state = new Mock<IDateComparison>(MockBehavior.Strict);

            state
                .Setup(p => p.AssertDate(It.IsAny<ITestFramework>(), It.IsAny<DateTime>(), It.IsAny<string>()))
                .Callback<ITestFramework, DateTime, string>((s, p, q) =>
                {
                    requestedValue = p;
                    requestedMessage = q;
                })
                .Throws(new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException());

            var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
                ValueValidation.Validate(_testFramework, state.Object, 123, "Custom assertion"));

            state
                .Verify(p => p.AssertDate(It.IsAny<ITestFramework>(), It.IsAny<DateTime>(), It.IsAny<string>()), Times.Never);

            Assert.IsNotNull(exception);
            Assert.AreEqual("Assert.IsInstanceOfType failed. Custom assertion is not a valid DateTime object Expected type:<System.DateTime>. Actual type:<System.Int32>.", exception.Message);
        }

        [Test]
        public void ValueValidation_Validate_AssertString_AreNotEqual_ThrowError()
        {
            string requestedValue = null;
            string requestedMessage = null;

            Mock<IStringComparison> state = new Mock<IStringComparison>(MockBehavior.Strict);

            state
                .Setup(p => p.AssertString(It.IsAny<ITestFramework>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback<ITestFramework, string, string>((s, p, q) =>
                {
                    requestedValue = p;
                    requestedMessage = q;
                })
                .Throws(new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException());

            Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
                ValueValidation.Validate(_testFramework, state.Object, "ABC", "Custom assertion"));

            state
                .Verify(p => p.AssertString(It.IsAny<ITestFramework>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            Assert.AreEqual("ABC", requestedValue);
            Assert.AreEqual("Custom assertion {0}", requestedMessage);
        }

        [Test]
        public void ValueValidation_Validate_AssertString_AreEqual_ThrowNoError()
        {
            string requestedValue = null;
            string requestedMessage = null;

            Mock<IStringComparison> state = new Mock<IStringComparison>(MockBehavior.Strict);

            state
                .Setup(p => p.AssertString(It.IsAny<ITestFramework>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback<ITestFramework, string, string>((s, p, q) =>
                {
                    requestedValue = p;
                    requestedMessage = q;
                });

            Assert.DoesNotThrow(() =>
                ValueValidation.Validate(_testFramework, state.Object, "ABC", "Custom assertion"));

            state
                .Verify(p => p.AssertString(It.IsAny<ITestFramework>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            Assert.AreEqual("ABC", requestedValue);
            Assert.AreEqual("Custom assertion {0}", requestedMessage);
        }

        [Test]
        public void ValueValidation_Validate_AssertString_ActualValueNotAString_ThrowError()
        {
            string requestedValue = null;
            string requestedMessage = null;

            Mock<IStringComparison> state = new Mock<IStringComparison>(MockBehavior.Strict);

            state
                .Setup(p => p.AssertString(It.IsAny<ITestFramework>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback<ITestFramework, string, string>((s, p, q) =>
                {
                    requestedValue = p;
                    requestedMessage = q;
                })
                .Throws(new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException());

            var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
                ValueValidation.Validate(_testFramework, state.Object, 123, "Custom assertion"));

            state
                .Verify(p => p.AssertString(It.IsAny<ITestFramework>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);

            Assert.IsNotNull(exception);
            Assert.AreEqual("Assert.IsInstanceOfType failed. Custom assertion is not a valid String object Expected type:<System.String>. Actual type:<System.Int32>.", exception.Message);
        }
    }
}

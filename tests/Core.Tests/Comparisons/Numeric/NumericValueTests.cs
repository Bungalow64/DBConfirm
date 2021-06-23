using DBConfirm.Core.Comparisons.Numeric;
using DBConfirm.Core.TestFrameworks.Abstract;
using DBConfirm.Frameworks.MSTest;
using NUnit.Framework;
using System;

namespace Core.Tests.Comparisons.Numeric
{
    [TestFixture]
    public class NumericValueTests
    {
        private readonly ITestFramework _testFramework = new MSTestFramework();

        [Test]
        public void NumericValue_CtorWithInt_ValueSet()
        {
            NumericValue numericValue = new NumericValue(123);
            Assert.AreEqual(123, numericValue.ExpectedValue);
        }

        [Test]
        public void NumericValue_CtorWithShort_ValueSet()
        {
            NumericValue numericValue = new NumericValue((short)20);
            Assert.AreEqual((short)20, numericValue.ExpectedValue);
        }

        [Test]
        public void NumericValue_CtorWithLong_ValueSet()
        {
            NumericValue numericValue = new NumericValue(20L);
            Assert.AreEqual(20L, numericValue.ExpectedValue);
        }

        [Test]
        public void NumericValue_CtorWithDecimal_ValueSet()
        {
            NumericValue numericValue = new NumericValue(20M);
            Assert.AreEqual(20L, numericValue.ExpectedValue);
        }

        [Test]
        public void NumericValue_CtorWithDouble_ValueSet()
        {
            NumericValue numericValue = new NumericValue(10D);
            Assert.AreEqual(10D, numericValue.ExpectedValue);
        }

        [Test]
        public void NumericValue_CtorWithFloat_ValueSet()
        {
            NumericValue numericValue = new NumericValue(10F);
            Assert.AreEqual(10D, numericValue.ExpectedValue);
        }

        [Test]
        public void NumericValue_CtorWithByte_ValueSet()
        {
            NumericValue numericValue = new NumericValue((byte)10);
            Assert.AreEqual((byte)10, numericValue.ExpectedValue);
        }

        [Test]
        public void NumericValue_Assert_ActualValueIsString_ThrowAreEqualException()
        {
            NumericValue numericValue = new NumericValue(10);

            Exception ex = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
                numericValue.Assert(_testFramework, "not a number", "Custom Number"));

            Assert.AreEqual("Assert.AreEqual failed. Expected:<10 (System.Int32)>. Actual:<not a number (System.String)>. Custom Number does not match expected value", ex.Message);
        }

        [Test]
        public void NumericValue_Assert_ActualValueIsNumberAsString_ThrowAreEqualException()
        {
            NumericValue numericValue = new NumericValue(10);

            Exception ex = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
                numericValue.Assert(_testFramework, "10", "Custom Number"));

            Assert.AreEqual("Assert.AreEqual failed. Expected:<10 (System.Int32)>. Actual:<10 (System.String)>. Custom Number does not match expected value", ex.Message);
        }

        [TestCase(0, typeof(int))]
        [TestCase(10, typeof(int))]
        [TestCase(-50, typeof(int))]
        [TestCase(0, typeof(short))]
        [TestCase(10, typeof(short))]
        [TestCase(-50, typeof(short))]
        [TestCase(0, typeof(long))]
        [TestCase(10, typeof(long))]
        [TestCase(-50, typeof(long))]
        [TestCase(0, typeof(decimal))]
        [TestCase(10, typeof(decimal))]
        [TestCase(-50, typeof(decimal))]
        [TestCase(0, typeof(double))]
        [TestCase(10, typeof(double))]
        [TestCase(-50, typeof(double))]
        [TestCase(0, typeof(float))]
        [TestCase(10, typeof(float))]
        [TestCase(-50, typeof(float))]
        [TestCase(0, typeof(byte))]
        [TestCase(10, typeof(byte))]
        [TestCase(50, typeof(byte))]
        public void NumericValue_Assert_DifferentTypes_ExpectedInt_SameNumericValues_NoException(int value, Type actualType)
        {
            NumericValue numericValue = new NumericValue(value);

            Assert.DoesNotThrow(() => numericValue.Assert(_testFramework, Convert.ChangeType(value, actualType), "Custom Number"));
        }

        [TestCase(0, 1, typeof(int))]
        [TestCase(10, 11, typeof(int))]
        [TestCase(-50, 50, typeof(int))]
        [TestCase(0, 1, typeof(short))]
        [TestCase(10, 11, typeof(short))]
        [TestCase(-50, 50, typeof(short))]
        [TestCase(0, 1, typeof(long))]
        [TestCase(10, 11, typeof(long))]
        [TestCase(-50, 50, typeof(long))]
        [TestCase(0, 1, typeof(decimal))]
        [TestCase(10, 11, typeof(decimal))]
        [TestCase(-50, 50, typeof(decimal))]
        [TestCase(0, 1, typeof(double))]
        [TestCase(10, 11, typeof(double))]
        [TestCase(-50, 50, typeof(double))]
        [TestCase(0, 1, typeof(float))]
        [TestCase(10, 11, typeof(float))]
        [TestCase(-50, 50, typeof(float))]
        [TestCase(0, 1, typeof(byte))]
        [TestCase(10, 11, typeof(byte))]
        [TestCase(50, 51, typeof(byte))]
        public void NumericValue_Assert_DifferentTypes_ExpectedInt_DifferentNumericValues_NoException(int expectedValue, int actualValue, Type actualType)
        {
            NumericValue numericValue = new NumericValue(expectedValue);

            Exception ex = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
                numericValue.Assert(_testFramework, Convert.ChangeType(actualValue, actualType), "Custom Number"));

            Assert.AreEqual($"Assert.AreEqual failed. Expected:<{expectedValue}>. Actual:<{actualValue}>. Custom Number does not match expected value", ex.Message);
        }

        [TestCase(0, typeof(decimal))]
        [TestCase(10, typeof(decimal))]
        [TestCase(-50, typeof(decimal))]
        [TestCase(0, typeof(int))]
        [TestCase(10, typeof(int))]
        [TestCase(-50, typeof(int))]
        [TestCase(0, typeof(short))]
        [TestCase(10, typeof(short))]
        [TestCase(-50, typeof(short))]
        [TestCase(0, typeof(long))]
        [TestCase(10, typeof(long))]
        [TestCase(-50, typeof(long))]
        [TestCase(0, typeof(double))]
        [TestCase(10, typeof(double))]
        [TestCase(-50, typeof(double))]
        [TestCase(0.1, typeof(double))]
        [TestCase(10.1, typeof(double))]
        [TestCase(-50.1, typeof(double))]
        [TestCase(0, typeof(float))]
        [TestCase(10, typeof(float))]
        [TestCase(-50, typeof(float))]
        [TestCase(0.1, typeof(float))]
        [TestCase(10.1, typeof(float))]
        [TestCase(-50.1, typeof(float))]
        [TestCase(0, typeof(byte))]
        [TestCase(10, typeof(byte))]
        [TestCase(50, typeof(byte))]
        public void NumericValue_Assert_DifferentTypes_ExpectedDecimal_SameNumericValues_NoException(decimal value, Type actualType)
        {
            NumericValue numericValue = new NumericValue(value);

            Assert.DoesNotThrow(() => numericValue.Assert(_testFramework, Convert.ChangeType(value, actualType), "Custom Number"));
        }

        [TestCase(0, 1, typeof(decimal))]
        [TestCase(10, 11, typeof(decimal))]
        [TestCase(-50, 50, typeof(decimal))]
        [TestCase(0.1, 0, typeof(decimal))]
        [TestCase(10.1, 10, typeof(decimal))]
        [TestCase(-50.1, -50, typeof(decimal))]
        [TestCase(0, 1, typeof(int))]
        [TestCase(10, 11, typeof(int))]
        [TestCase(-50, 50, typeof(int))]
        [TestCase(0, 1, typeof(short))]
        [TestCase(10, 11, typeof(short))]
        [TestCase(-50, 50, typeof(short))]
        [TestCase(0, 1, typeof(long))]
        [TestCase(10, 11, typeof(long))]
        [TestCase(-50, 50, typeof(long))]
        [TestCase(0, 1, typeof(double))]
        [TestCase(10, 11, typeof(double))]
        [TestCase(-50, 50, typeof(double))]
        [TestCase(0.1, 0, typeof(double))]
        [TestCase(10.1, 10, typeof(double))]
        [TestCase(-50.1, -50, typeof(double))]
        [TestCase(0, 1, typeof(float))]
        [TestCase(10, 11, typeof(float))]
        [TestCase(-50, 50, typeof(float))]
        [TestCase(0.1, 0, typeof(float))]
        [TestCase(10.1, 10, typeof(float))]
        [TestCase(-50.1, -50, typeof(float))]
        [TestCase(0, 1, typeof(byte))]
        [TestCase(10, 11, typeof(byte))]
        [TestCase(50, 51, typeof(byte))]
        public void NumericValue_Assert_DifferentTypes_ExpectedInt_DifferentNumericValues_NoException(decimal expectedValue, decimal actualValue, Type actualType)
        {
            NumericValue numericValue = new NumericValue(expectedValue);

            Exception ex = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
                numericValue.Assert(_testFramework, Convert.ChangeType(actualValue, actualType), "Custom Number"));

            Assert.AreEqual($"Assert.AreEqual failed. Expected:<{expectedValue}>. Actual:<{actualValue}>. Custom Number does not match expected value", ex.Message);
        }

        [TestCase(0.1, 0, typeof(int))]
        [TestCase(10.1, 10, typeof(int))]
        [TestCase(-50.1, -50, typeof(int))]
        public void NumericValue_Assert_DifferentTypes_ExpectedInt_DifferentNumericValuesNotCastable_NoException(decimal expectedValue, decimal actualValue, Type actualType)
        {
            NumericValue numericValue = new NumericValue(expectedValue);

            Exception ex = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
                numericValue.Assert(_testFramework, Convert.ChangeType(actualValue, actualType), "Custom Number"));

            Assert.AreEqual($"Assert.AreEqual failed. Expected:<{expectedValue} (System.Decimal)>. Actual:<{actualValue} (System.Int32)>. Custom Number does not match expected value", ex.Message);
        }

        [Test]
        public void NumericValue_Validate_ActualValueIsString_ThrowAreEqualException()
        {
            NumericValue numericValue = new NumericValue(10);

            bool result = numericValue.Validate("not a number");

            Assert.False(result);
        }

        [Test]
        public void NumericValue_Validate_ActualValueIsNumberAsString_ThrowAreEqualException()
        {
            NumericValue numericValue = new NumericValue(10);

            bool result = numericValue.Validate("10");

            Assert.False(result);
        }

        [TestCase(0, typeof(int))]
        [TestCase(10, typeof(int))]
        [TestCase(-50, typeof(int))]
        [TestCase(0, typeof(short))]
        [TestCase(10, typeof(short))]
        [TestCase(-50, typeof(short))]
        [TestCase(0, typeof(long))]
        [TestCase(10, typeof(long))]
        [TestCase(-50, typeof(long))]
        [TestCase(0, typeof(decimal))]
        [TestCase(10, typeof(decimal))]
        [TestCase(-50, typeof(decimal))]
        [TestCase(0, typeof(double))]
        [TestCase(10, typeof(double))]
        [TestCase(-50, typeof(double))]
        [TestCase(0, typeof(float))]
        [TestCase(10, typeof(float))]
        [TestCase(-50, typeof(float))]
        [TestCase(0, typeof(byte))]
        [TestCase(10, typeof(byte))]
        [TestCase(50, typeof(byte))]
        public void NumericValue_Validate_DifferentTypes_ExpectedInt_SameNumericValues_NoException(int value, Type actualType)
        {
            NumericValue numericValue = new NumericValue(value);

            bool result = numericValue.Validate(Convert.ChangeType(value, actualType));

            Assert.True(result);
        }

        [TestCase(0, 1, typeof(int))]
        [TestCase(10, 11, typeof(int))]
        [TestCase(-50, 50, typeof(int))]
        [TestCase(0, 1, typeof(short))]
        [TestCase(10, 11, typeof(short))]
        [TestCase(-50, 50, typeof(short))]
        [TestCase(0, 1, typeof(long))]
        [TestCase(10, 11, typeof(long))]
        [TestCase(-50, 50, typeof(long))]
        [TestCase(0, 1, typeof(decimal))]
        [TestCase(10, 11, typeof(decimal))]
        [TestCase(-50, 50, typeof(decimal))]
        [TestCase(0, 1, typeof(double))]
        [TestCase(10, 11, typeof(double))]
        [TestCase(-50, 50, typeof(double))]
        [TestCase(0, 1, typeof(float))]
        [TestCase(10, 11, typeof(float))]
        [TestCase(-50, 50, typeof(float))]
        [TestCase(0, 1, typeof(byte))]
        [TestCase(10, 11, typeof(byte))]
        [TestCase(50, 51, typeof(byte))]
        public void NumericValue_Validate_DifferentTypes_ExpectedInt_DifferentNumericValues_NoException(int expectedValue, int actualValue, Type actualType)
        {
            NumericValue numericValue = new NumericValue(expectedValue);

            bool result = numericValue.Validate(Convert.ChangeType(actualValue, actualType));

            Assert.False(result);
        }

        [TestCase(0, typeof(decimal))]
        [TestCase(10, typeof(decimal))]
        [TestCase(-50, typeof(decimal))]
        [TestCase(0, typeof(int))]
        [TestCase(10, typeof(int))]
        [TestCase(-50, typeof(int))]
        [TestCase(0, typeof(short))]
        [TestCase(10, typeof(short))]
        [TestCase(-50, typeof(short))]
        [TestCase(0, typeof(long))]
        [TestCase(10, typeof(long))]
        [TestCase(-50, typeof(long))]
        [TestCase(0, typeof(double))]
        [TestCase(10, typeof(double))]
        [TestCase(-50, typeof(double))]
        [TestCase(0.1, typeof(double))]
        [TestCase(10.1, typeof(double))]
        [TestCase(-50.1, typeof(double))]
        [TestCase(0, typeof(float))]
        [TestCase(10, typeof(float))]
        [TestCase(-50, typeof(float))]
        [TestCase(0.1, typeof(float))]
        [TestCase(10.1, typeof(float))]
        [TestCase(-50.1, typeof(float))]
        [TestCase(0, typeof(byte))]
        [TestCase(10, typeof(byte))]
        [TestCase(50, typeof(byte))]
        public void NumericValue_Validate_DifferentTypes_ExpectedDecimal_SameNumericValues_NoException(decimal value, Type actualType)
        {
            NumericValue numericValue = new NumericValue(value);

            bool result = numericValue.Validate(Convert.ChangeType(value, actualType));

            Assert.True(result);
        }

        [TestCase(0, 1, typeof(decimal))]
        [TestCase(10, 11, typeof(decimal))]
        [TestCase(-50, 50, typeof(decimal))]
        [TestCase(0.1, 0, typeof(decimal))]
        [TestCase(10.1, 10, typeof(decimal))]
        [TestCase(-50.1, -50, typeof(decimal))]
        [TestCase(0, 1, typeof(int))]
        [TestCase(10, 11, typeof(int))]
        [TestCase(-50, 50, typeof(int))]
        [TestCase(0.1, 0, typeof(int))]
        [TestCase(10.1, 10, typeof(int))]
        [TestCase(-50.1, -50, typeof(int))]
        [TestCase(0, 1, typeof(short))]
        [TestCase(10, 11, typeof(short))]
        [TestCase(-50, 50, typeof(short))]
        [TestCase(0, 1, typeof(long))]
        [TestCase(10, 11, typeof(long))]
        [TestCase(-50, 50, typeof(long))]
        [TestCase(0, 1, typeof(double))]
        [TestCase(10, 11, typeof(double))]
        [TestCase(-50, 50, typeof(double))]
        [TestCase(0.1, 0, typeof(double))]
        [TestCase(10.1, 10, typeof(double))]
        [TestCase(-50.1, -50, typeof(double))]
        [TestCase(0, 1, typeof(float))]
        [TestCase(10, 11, typeof(float))]
        [TestCase(-50, 50, typeof(float))]
        [TestCase(0.1, 0, typeof(float))]
        [TestCase(10.1, 10, typeof(float))]
        [TestCase(-50.1, -50, typeof(float))]
        [TestCase(0, 1, typeof(byte))]
        [TestCase(10, 11, typeof(byte))]
        [TestCase(50, 51, typeof(byte))]
        public void NumericValue_Validate_DifferentTypes_ExpectedInt_DifferentNumericValues_NoException(decimal expectedValue, decimal actualValue, Type actualType)
        {
            NumericValue numericValue = new NumericValue(expectedValue);

            bool result = numericValue.Validate(Convert.ChangeType(actualValue, actualType));

            Assert.False(result);
        }
    }
}

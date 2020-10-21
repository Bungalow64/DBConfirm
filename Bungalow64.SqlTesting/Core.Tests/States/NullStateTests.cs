using SQLConfirm.Core.Comparisons.States;
using SQLConfirm.Core.TestFrameworks.Abstract;
using NUnit.Framework;
using SQLConfirm.Frameworks.MSTest2;
using System;

namespace SqlConfirm.Core.Tests.States
{
    [TestFixture]
    public class NullStateTests
    {
        private readonly ITestFramework _testFramework = new MSTest2Framework();

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
}

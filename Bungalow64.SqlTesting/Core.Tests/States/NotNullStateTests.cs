using SQLConfirm.Core.Comparisons.States;
using SQLConfirm.Core.TestFrameworks.Abstract;
using NUnit.Framework;
using SQLConfirm.Frameworks.MSTest2;
using System;

namespace SqlConfirm.Core.Tests.States
{
    [TestFixture]
    public class NotNullStateTests
    {
        private readonly ITestFramework _testFramework = new MSTest2Framework();

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
}

using Models.States;
using NUnit.Framework;
using System;

namespace Models.Tests.States
{
    [TestFixture]
    public class NotNullStateTests
    {
        [Test]
        public void NotNullState_HasValue_NoError()
        {
            object value = 123;

            new NotNullState()
                .AssertState(value, "CustomMessage");
        }

        [Test]
        public void NotNullState_HasNullValue_Error()
        {
            object value = null;

            var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() => new NotNullState()
                .AssertState(value, "CustomMessage"));

            Assert.AreEqual("Assert.AreNotEqual failed. Expected any value except:<>. Actual:<>. CustomMessage", exception.Message);
        }

        [Test]
        public void NotNullState_HasDBNullValue_Error()
        {
            object value = DBNull.Value;

            var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() => new NotNullState()
                .AssertState(value, "CustomMessage"));

            Assert.AreEqual("Assert.AreNotEqual failed. Expected any value except:<>. Actual:<>. CustomMessage", exception.Message);
        }
    }
}

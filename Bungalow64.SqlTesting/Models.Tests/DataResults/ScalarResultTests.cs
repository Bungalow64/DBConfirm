using Models.DataResults;
using NUnit.Framework;

namespace Models.Tests.DataResults
{
    [TestFixture]
    public class ScalarResultTests
    {
        [Test]
        public void ScalarResult_Ctor_ValueSet()
        {
            ScalarResult<int> result = new ScalarResult<int>(1001);

            Assert.AreEqual(1001, result.RawData);
        }

        [Test]
        public void ScalarResult_AssertValue_ValueMatches_NoError()
        {
            ScalarResult<int> result = new ScalarResult<int>(1001);

            Assert.DoesNotThrow(() => 
                result.AssertValue(1001));
        }

        [Test]
        public void ScalarResult_AssertValue_ValueDoesNotMatch_Error()
        {
            ScalarResult<int> result = new ScalarResult<int>(1001);

            var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
               result.AssertValue(1002));

            Assert.AreEqual("Assert.AreEqual failed. Expected:<1002>. Actual:<1001>. Scalar result has an unexpected value", exception.Message);
        }

        [Test]
        public void ScalarResult_AssertValue_ValueDoesNotMatchExpectedIsNull_Error()
        {
            ScalarResult<int> result = new ScalarResult<int>(1001);

            var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
               result.AssertValue(null));

            Assert.AreEqual("Assert.AreEqual failed. Expected:< (System.DBNull)>. Actual:<1001 (System.Int32)>. Scalar result has an unexpected value", exception.Message);
        }
    }
}

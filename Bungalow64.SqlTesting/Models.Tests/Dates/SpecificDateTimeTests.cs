using Models.Comparisons.Dates;
using Models.TestFrameworks.Abstract;
using NUnit.Framework;
using System;
using System.Globalization;
using System.Threading;

namespace Models.Tests.Dates
{
    [TestFixture]
    public class SpecificDateTimeTests
    {
        private readonly ITestFramework _testFramework = new Frameworks.MSTest2.MSTest2Framework();

        [OneTimeSetUp]
        public void Setup()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
        }

        [Test]
        public void SpecificDateTime_CtorWithDate_DateSet()
        {
            SpecificDateTime date = new SpecificDateTime(DateTime.Parse("01-Mar-2020"));
            Assert.AreEqual(DateTime.Parse("01-Mar-2020"), date.ExpectedDate);
        }

        [Test]
        public void SpecificDateTime_CtorWithDateAsString_DateSet()
        {
            SpecificDateTime date = new SpecificDateTime("01-Mar-2020");
            Assert.AreEqual(DateTime.Parse("01-Mar-2020"), date.ExpectedDate);
        }

        [Test]
        public void SpecificDateTime_Ctor_DefaultPrecisionIs1Second()
        {
            SpecificDateTime date = new SpecificDateTime(DateTime.Parse("01-Mar-2020"));
            Assert.AreEqual(TimeSpan.FromSeconds(1), date.Precision);
        }

        [TestCase("01-Mar-2020", "01-Mar-2020")]
        [TestCase("01-Mar-2020 15:12:31", "01-Mar-2020 15:12:31")]
        public void SpecificDateTime_AssertDate_DefaultPrecision_SameDate_NoError(string expectedDateString, string actualDateString)
        {
            DateTime expectedDate = DateTime.Parse(expectedDateString);
            DateTime actualDate = DateTime.Parse(actualDateString);

            Assert.DoesNotThrow(() =>
                new SpecificDateTime(expectedDate)
                    .Assert(_testFramework, actualDate, "DateTime is wrong: {0}"));
        }

        [TestCase("02-Mar-2020", "01-Mar-2020", "Assert.AreEqual failed. Expected:<02/03/2020 00:00:00>. Actual:<01/03/2020 00:00:00>. Custom Date is different by -86400000 ms")]
        [TestCase("01-Mar-2020", "02-Mar-2020", "Assert.AreEqual failed. Expected:<01/03/2020 00:00:00>. Actual:<02/03/2020 00:00:00>. Custom Date is different by 86400000 ms")]
        [TestCase("03-Mar-2020", "01-Mar-2020", "Assert.AreEqual failed. Expected:<03/03/2020 00:00:00>. Actual:<01/03/2020 00:00:00>. Custom Date is different by -172800000 ms")]
        [TestCase("01-Mar-2020", "03-Mar-2020", "Assert.AreEqual failed. Expected:<01/03/2020 00:00:00>. Actual:<03/03/2020 00:00:00>. Custom Date is different by 172800000 ms")]
        [TestCase("01-Mar-2020 08:12:34", "03-Mar-2020 15:12:31", "Assert.AreEqual failed. Expected:<01/03/2020 08:12:34>. Actual:<03/03/2020 15:12:31>. Custom Date is different by 197997000 ms")]
        [TestCase("01-Mar-2020 09:12:31", "01-Mar-2020 21:23:54", "Assert.AreEqual failed. Expected:<01/03/2020 09:12:31>. Actual:<01/03/2020 21:23:54>. Custom Date is different by 43883000 ms")]
        [TestCase("01-Mar-2020 00:00:00", "01-Mar-2020 23:59:59", "Assert.AreEqual failed. Expected:<01/03/2020 00:00:00>. Actual:<01/03/2020 23:59:59>. Custom Date is different by 86399000 ms")]
        [TestCase("01-Mar-2020 00:00:00", "01-Mar-2020 00:00:01", "Assert.AreEqual failed. Expected:<01/03/2020 00:00:00>. Actual:<01/03/2020 00:00:01>. Custom Date is different by 1000 ms")]
        [TestCase("01-Mar-2020 00:00:01", "01-Mar-2020 00:00:00", "Assert.AreEqual failed. Expected:<01/03/2020 00:00:01>. Actual:<01/03/2020 00:00:00>. Custom Date is different by -1000 ms")]
        public void SpecificDateTime_AssertDate_DifferentTimes_DefaultPrecision_Error(string expectedDateString, string actualDateString, string expectedMessage)
        {
            DateTime expectedDate = DateTime.Parse(expectedDateString);
            DateTime actualDate = DateTime.Parse(actualDateString);

            Exception ex = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
                new SpecificDateTime(expectedDate)
                    .Assert(_testFramework, actualDate, "Custom Date"));

            Assert.AreEqual(expectedMessage, ex.Message);
        }

        [TestCase("01-Mar-2020", "01-Mar-2020")]
        [TestCase("01-Mar-2020 15:12:20", "01-Mar-2020 15:12:40")]
        [TestCase("01-Mar-2020 15:12:00", "01-Mar-2020 15:12:59")]
        [TestCase("01-Mar-2020 00:00:00", "01-Mar-2020 00:00:01")]
        [TestCase("01-Mar-2020 00:00:01", "01-Mar-2020 00:00:00")]
        public void SpecificDateTime_AssertDate_Precision1Minute_SameDate_NoError(string expectedDateString, string actualDateString)
        {
            DateTime expectedDate = DateTime.Parse(expectedDateString);
            DateTime actualDate = DateTime.Parse(actualDateString);

            Assert.DoesNotThrow(() =>
                new SpecificDateTime(expectedDate, TimeSpan.FromMinutes(1))
                    .Assert(_testFramework, actualDate, "Custom Date"));
        }

        [TestCase("02-Mar-2020", "01-Mar-2020", "Assert.AreEqual failed. Expected:<02/03/2020 00:00:00>. Actual:<01/03/2020 00:00:00>. Custom Date is different by -86400000 ms")]
        [TestCase("01-Mar-2020", "02-Mar-2020", "Assert.AreEqual failed. Expected:<01/03/2020 00:00:00>. Actual:<02/03/2020 00:00:00>. Custom Date is different by 86400000 ms")]
        [TestCase("03-Mar-2020", "01-Mar-2020", "Assert.AreEqual failed. Expected:<03/03/2020 00:00:00>. Actual:<01/03/2020 00:00:00>. Custom Date is different by -172800000 ms")]
        [TestCase("01-Mar-2020", "03-Mar-2020", "Assert.AreEqual failed. Expected:<01/03/2020 00:00:00>. Actual:<03/03/2020 00:00:00>. Custom Date is different by 172800000 ms")]
        [TestCase("01-Mar-2020 08:12:34", "03-Mar-2020 15:12:31", "Assert.AreEqual failed. Expected:<01/03/2020 08:12:34>. Actual:<03/03/2020 15:12:31>. Custom Date is different by 197997000 ms")]
        [TestCase("01-Mar-2020 09:12:31", "01-Mar-2020 21:23:54", "Assert.AreEqual failed. Expected:<01/03/2020 09:12:31>. Actual:<01/03/2020 21:23:54>. Custom Date is different by 43883000 ms")]
        [TestCase("01-Mar-2020 00:00:00", "01-Mar-2020 23:59:59", "Assert.AreEqual failed. Expected:<01/03/2020 00:00:00>. Actual:<01/03/2020 23:59:59>. Custom Date is different by 86399000 ms")]
        [TestCase("01-Mar-2020 00:00:00", "01-Mar-2020 00:01:00", "Assert.AreEqual failed. Expected:<01/03/2020 00:00:00>. Actual:<01/03/2020 00:01:00>. Custom Date is different by 60000 ms")]
        [TestCase("01-Mar-2020 00:01:00", "01-Mar-2020 00:00:00", "Assert.AreEqual failed. Expected:<01/03/2020 00:01:00>. Actual:<01/03/2020 00:00:00>. Custom Date is different by -60000 ms")]
        public void SpecificDateTime_AssertDate_DifferentTimes_Precision1Minute_Error(string expectedDateString, string actualDateString, string expectedMessage)
        {
            DateTime expectedDate = DateTime.Parse(expectedDateString);
            DateTime actualDate = DateTime.Parse(actualDateString);

            Exception ex = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
                new SpecificDateTime(expectedDate)
                    .Assert(_testFramework, actualDate, "Custom Date"));

            Assert.AreEqual(expectedMessage, ex.Message);
        }
    }
}

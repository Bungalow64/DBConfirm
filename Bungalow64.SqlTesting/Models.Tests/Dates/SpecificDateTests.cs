using Models.Dates;
using Models.TestFrameworks.Abstract;
using NUnit.Framework;
using System;
using System.Globalization;
using System.Threading;

namespace Models.Tests.Dates
{
    [TestFixture]
    public class SpecificDateTests
    {
        private readonly ITestFramework _testFramework = new Frameworks.MSTest2.MSTest2Framework();

        [OneTimeSetUp]
        public void Setup()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
        }

        [Test]
        public void SpecificDate_CtorWithDate_DateSet()
        {
            SpecificDate date = new SpecificDate(DateTime.Parse("01-Mar-2020"));
            Assert.AreEqual(DateTime.Parse("01-Mar-2020"), date.ExpectedDate);
        }

        [Test]
        public void SpecificDate_CtorWithDateAsString_DateSet()
        {
            SpecificDate date = new SpecificDate("01-Mar-2020");
            Assert.AreEqual(DateTime.Parse("01-Mar-2020"), date.ExpectedDate);
        }

        [Test]
        public void SpecificDate_Ctor_DefaultPrecisionIsZero()
        {
            SpecificDate date = new SpecificDate(DateTime.Parse("01-Mar-2020"));
            Assert.AreEqual(TimeSpan.Zero, date.Precision);
        }

        [TestCase("01-Mar-2020", "01-Mar-2020")]
        [TestCase("01-Mar-2020 09:12:31", "01-Mar-2020 21:23:54")]
        [TestCase("01-Mar-2020 00:00:00", "01-Mar-2020 23:59:59")]
        public void SpecificDate_AssertDate_SameDate_NoError(string expectedDateString, string actualDateString)
        {
            DateTime expectedDate = DateTime.Parse(expectedDateString);
            DateTime actualDate = DateTime.Parse(actualDateString);

            Assert.DoesNotThrow(() =>
                new SpecificDate(expectedDate)
                    .AssertDate(_testFramework, actualDate, "Date is wrong: {0}"));
        }

        [TestCase("02-Mar-2020", "01-Mar-2020", "Assert.AreEqual failed. Expected:<02/03/2020 00:00:00>. Actual:<01/03/2020 00:00:00>. Date is wrong: -1 day")]
        [TestCase("01-Mar-2020", "02-Mar-2020", "Assert.AreEqual failed. Expected:<01/03/2020 00:00:00>. Actual:<02/03/2020 00:00:00>. Date is wrong: 1 day")]
        [TestCase("03-Mar-2020", "01-Mar-2020", "Assert.AreEqual failed. Expected:<03/03/2020 00:00:00>. Actual:<01/03/2020 00:00:00>. Date is wrong: -2 days")]
        [TestCase("01-Mar-2020", "03-Mar-2020", "Assert.AreEqual failed. Expected:<01/03/2020 00:00:00>. Actual:<03/03/2020 00:00:00>. Date is wrong: 2 days")]
        [TestCase("01-Mar-2020 08:12:34", "03-Mar-2020 15:12:31", "Assert.AreEqual failed. Expected:<01/03/2020 00:00:00>. Actual:<03/03/2020 00:00:00>. Date is wrong: 2 days")]
        [TestCase("01-Mar-2020 23:59:59", "02-Mar-2020 00:00:00", "Assert.AreEqual failed. Expected:<01/03/2020 00:00:00>. Actual:<02/03/2020 00:00:00>. Date is wrong: 1 day")]
        public void SpecificDate_AssertDate_DifferentDates_Error(string expectedDateString, string actualDateString, string expectedMessage)
        {
            DateTime expectedDate = DateTime.Parse(expectedDateString);
            DateTime actualDate = DateTime.Parse(actualDateString);

            Exception ex = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
                new SpecificDate(expectedDate)
                    .AssertDate(_testFramework, actualDate, "Date is wrong: {0}"));

            Assert.AreEqual(expectedMessage, ex.Message);
        }
    }
}

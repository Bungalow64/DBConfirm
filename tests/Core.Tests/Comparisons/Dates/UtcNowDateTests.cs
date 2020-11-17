using DBConfirm.Core.Comparisons.Dates;
using DBConfirm.Core.Factories.Abstract;
using DBConfirm.Core.TestFrameworks.Abstract;
using Moq;
using NUnit.Framework;
using DBConfirm.Frameworks.MSTest;
using System;
using System.Globalization;
using System.Threading;

namespace Core.Tests.Comparisons.Dates
{
    [TestFixture]
    public class UtcNowDateTests
    {
        private readonly ITestFramework _testFramework = new MSTestFramework();
        private Mock<IDateUtcNowFactory> _dateUtcNowFactoryMock;

        [OneTimeSetUp]
        public void Setup()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
        }

        [SetUp]
        public void TestInit()
        {
            _dateUtcNowFactoryMock = new Mock<IDateUtcNowFactory>(MockBehavior.Strict);
        }

        private UtcNowDate CreateUtcNowDate(TimeSpan? precision = null)
        {
            if (precision.HasValue)
            {
                return new UtcNowDate(precision.Value)
                {
                    DateUtcNowFactory = _dateUtcNowFactoryMock.Object
                };
            }
            return new UtcNowDate
            {
                DateUtcNowFactory = _dateUtcNowFactoryMock.Object
            };
        }

        [Test]
        public void UtcNowDate_Ctor_NoError()
        {
            new UtcNowDate();
        }

        [Test]
        public void UtcNowDate_Ctor_DefaultPrecisionIs1Second()
        {
            UtcNowDate date = new UtcNowDate();
            Assert.AreEqual(TimeSpan.FromSeconds(1), date.Precision);
        }

        [TestCase("01-Mar-2020", "01-Mar-2020")]
        [TestCase("01-Mar-2020 00:00:00.000", "01-Mar-2020 00:00:00.010")]
        [TestCase("01-Mar-2020 00:00:00.010", "01-Mar-2020 00:00:00.000")]
        [TestCase("04-Mar-2020 08:10:10.500", "04-Mar-2020 08:10:10.600")]
        [TestCase("04-Mar-2020 08:10:10.500", "04-Mar-2020 08:10:10.400")]
        [TestCase("04-Mar-2020 08:10:10.500", "04-Mar-2020 08:10:10.501")]
        [TestCase("04-Mar-2020 08:10:10.500", "04-Mar-2020 08:10:10.499")]
        public void UtcNowDate_AssertDate_IsUtcNow_DefaultPrecision_NoError(string utcNowDate, string actualDateString)
        {
            _dateUtcNowFactoryMock
                .SetupGet(p => p.UtcNow)
                .Returns(DateTime.Parse(utcNowDate));

            DateTime actualDate = DateTime.Parse(actualDateString);

            Assert.DoesNotThrow(() =>
                CreateUtcNowDate()
                    .Assert(_testFramework, actualDate, "Custom Date"));
        }

        [TestCase("04-Mar-2020 08:10:10.500", "03-Mar-2020 08:10:10.500", "Assert.AreEqual failed. Expected:<04/03/2020 08:10:10>. Actual:<03/03/2020 08:10:10>. Custom Date is different by -86400000 ms")]
        [TestCase("04-Mar-2020 08:10:10.500", "02-Mar-2020 08:10:10.500", "Assert.AreEqual failed. Expected:<04/03/2020 08:10:10>. Actual:<02/03/2020 08:10:10>. Custom Date is different by -172800000 ms")]
        [TestCase("04-Mar-2020 08:10:10.500", "05-Mar-2020 08:10:10.500", "Assert.AreEqual failed. Expected:<04/03/2020 08:10:10>. Actual:<05/03/2020 08:10:10>. Custom Date is different by 86400000 ms")]
        [TestCase("04-Mar-2020 08:10:10.500", "06-Mar-2020 08:10:10.500", "Assert.AreEqual failed. Expected:<04/03/2020 08:10:10>. Actual:<06/03/2020 08:10:10>. Custom Date is different by 172800000 ms")]
        [TestCase("04-Mar-2020 08:10:10.500", "04-Mar-2020 09:10:10.500", "Assert.AreEqual failed. Expected:<04/03/2020 08:10:10>. Actual:<04/03/2020 09:10:10>. Custom Date is different by 3600000 ms")]
        [TestCase("04-Mar-2020 08:10:10.500", "04-Mar-2020 07:10:10.500", "Assert.AreEqual failed. Expected:<04/03/2020 08:10:10>. Actual:<04/03/2020 07:10:10>. Custom Date is different by -3600000 ms")]
        [TestCase("04-Mar-2020 08:10:10.500", "04-Mar-2020 08:11:10.500", "Assert.AreEqual failed. Expected:<04/03/2020 08:10:10>. Actual:<04/03/2020 08:11:10>. Custom Date is different by 60000 ms")]
        [TestCase("04-Mar-2020 08:10:10.500", "04-Mar-2020 08:09:10.500", "Assert.AreEqual failed. Expected:<04/03/2020 08:10:10>. Actual:<04/03/2020 08:09:10>. Custom Date is different by -60000 ms")]
        [TestCase("04-Mar-2020 08:10:10.500", "04-Mar-2020 08:10:11.500", "Assert.AreEqual failed. Expected:<04/03/2020 08:10:10>. Actual:<04/03/2020 08:10:11>. Custom Date is different by 1000 ms")]
        [TestCase("04-Mar-2020 08:10:10.500", "04-Mar-2020 08:10:09.500", "Assert.AreEqual failed. Expected:<04/03/2020 08:10:10>. Actual:<04/03/2020 08:10:09>. Custom Date is different by -1000 ms")]
        public void UtcNowDate_AssertDate_DifferentTimes_DefaultPrecision_Error(string utcNowDate, string actualDateString, string expectedMessage)
        {
            _dateUtcNowFactoryMock
                .SetupGet(p => p.UtcNow)
                .Returns(DateTime.Parse(utcNowDate));

            DateTime actualDate = DateTime.Parse(actualDateString);

            Exception ex = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
                CreateUtcNowDate()
                    .Assert(_testFramework, actualDate, "Custom Date"));

            Assert.AreEqual(expectedMessage, ex.Message);
        }

        [TestCase("01-Mar-2020", "01-Mar-2020")]
        [TestCase("01-Mar-2020 00:00:00.000", "01-Mar-2020 00:00:00.010")]
        [TestCase("01-Mar-2020 00:00:00.010", "01-Mar-2020 00:00:00.000")]
        [TestCase("04-Mar-2020 08:10:10.500", "04-Mar-2020 08:10:10.600")]
        [TestCase("04-Mar-2020 08:10:10.500", "04-Mar-2020 08:10:10.400")]
        [TestCase("04-Mar-2020 08:10:10.500", "04-Mar-2020 08:10:10.501")]
        [TestCase("04-Mar-2020 08:10:10.500", "04-Mar-2020 08:10:10.499")]
        [TestCase("04-Mar-2020 08:10:10.500", "04-Mar-2020 08:10:11.500")]
        [TestCase("04-Mar-2020 08:10:10.500", "04-Mar-2020 08:10:09.500")]
        public void UtcNowDate_AssertDate_IsUtcNow_Precision1Minute_NoError(string utcNowDate, string actualDateString)
        {
            _dateUtcNowFactoryMock
                .SetupGet(p => p.UtcNow)
                .Returns(DateTime.Parse(utcNowDate));

            DateTime actualDate = DateTime.Parse(actualDateString);

            Assert.DoesNotThrow(() =>
                CreateUtcNowDate(TimeSpan.FromMinutes(1))
                    .Assert(_testFramework, actualDate, "Custom Date"));
        }

        [TestCase("04-Mar-2020 08:10:10.500", "03-Mar-2020 08:10:10.500", "Assert.AreEqual failed. Expected:<04/03/2020 08:10:10>. Actual:<03/03/2020 08:10:10>. Custom Date is different by -86400000 ms")]
        [TestCase("04-Mar-2020 08:10:10.500", "02-Mar-2020 08:10:10.500", "Assert.AreEqual failed. Expected:<04/03/2020 08:10:10>. Actual:<02/03/2020 08:10:10>. Custom Date is different by -172800000 ms")]
        [TestCase("04-Mar-2020 08:10:10.500", "05-Mar-2020 08:10:10.500", "Assert.AreEqual failed. Expected:<04/03/2020 08:10:10>. Actual:<05/03/2020 08:10:10>. Custom Date is different by 86400000 ms")]
        [TestCase("04-Mar-2020 08:10:10.500", "06-Mar-2020 08:10:10.500", "Assert.AreEqual failed. Expected:<04/03/2020 08:10:10>. Actual:<06/03/2020 08:10:10>. Custom Date is different by 172800000 ms")]
        [TestCase("04-Mar-2020 08:10:10.500", "04-Mar-2020 09:10:10.500", "Assert.AreEqual failed. Expected:<04/03/2020 08:10:10>. Actual:<04/03/2020 09:10:10>. Custom Date is different by 3600000 ms")]
        [TestCase("04-Mar-2020 08:10:10.500", "04-Mar-2020 07:10:10.500", "Assert.AreEqual failed. Expected:<04/03/2020 08:10:10>. Actual:<04/03/2020 07:10:10>. Custom Date is different by -3600000 ms")]
        [TestCase("04-Mar-2020 08:10:10.500", "04-Mar-2020 08:11:10.500", "Assert.AreEqual failed. Expected:<04/03/2020 08:10:10>. Actual:<04/03/2020 08:11:10>. Custom Date is different by 60000 ms")]
        [TestCase("04-Mar-2020 08:10:10.500", "04-Mar-2020 08:09:10.500", "Assert.AreEqual failed. Expected:<04/03/2020 08:10:10>. Actual:<04/03/2020 08:09:10>. Custom Date is different by -60000 ms")]
        public void UtcNowDate_AssertDate_DifferentTimes_Precision1Minute_Error(string utcNowDate, string actualDateString, string expectedMessage)
        {
            _dateUtcNowFactoryMock
                .SetupGet(p => p.UtcNow)
                .Returns(DateTime.Parse(utcNowDate));

            DateTime actualDate = DateTime.Parse(actualDateString);

            Exception ex = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
                CreateUtcNowDate(TimeSpan.FromMinutes(1))
                    .Assert(_testFramework, actualDate, "Custom Date"));

            Assert.AreEqual(expectedMessage, ex.Message);
        }
    }
}

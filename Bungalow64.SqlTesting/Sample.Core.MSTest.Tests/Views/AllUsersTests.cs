using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using System;
using Models.DataResults;
using System.Threading.Tasks;
using Frameworks.MSTest2;
using Sample.Core.MSTest.Tests.Templates;
using Sample.Core.MSTest.Tests.Templates.Complex;

namespace Sample.Core.MSTest.Tests.Views
{
    [TestClass]
    public class AllUsersTests : TestBase
    {
        [TestMethod]
        public async Task AllUsers_NoData_NothingReturned()
        {
            QueryResult results = await TestRunner.ExecuteViewAsync("dbo.AllUsers");

            results
                .AssertRowCount(0);

            Assert.AreEqual(0, await TestRunner.CountRowsInViewAsync("dbo.AllUsers"));
        }

        [TestMethod]
        public async Task AllUsers_OneRow_OneUserReturned()
        {
            await TestRunner.ExecuteStoredProcedureNonQueryAsync("dbo.AddUser",
                new SqlParameter("FirstName", "Jamie"),
                new SqlParameter("LastName", "Burns"),
                new SqlParameter("EmailAddress", "jamie@bungalow64.co.uk"),
                new SqlParameter("StartDate", DateTime.Parse("01-Mar-2020")),
                new SqlParameter("NumberOfHats", 14),
                new SqlParameter("Cost", 15.87));

            QueryResult results = await TestRunner.ExecuteViewAsync("dbo.AllUsers");

            results
                .AssertRowCount(1)
                .AssertRowValues(0, new DataSetRow
                {
                    { "FirstName", "Jamie" }
                });

            Assert.AreEqual(1, await TestRunner.CountRowsInViewAsync("dbo.AllUsers"));
        }

        [TestMethod]
        public async Task AllUsers_TwoRows_TwoUsersReturned()
        {
            await TestRunner.ExecuteStoredProcedureNonQueryAsync("dbo.AddUser",
                new SqlParameter("FirstName", "Jamie"),
                new SqlParameter("LastName", "Burns"),
                new SqlParameter("EmailAddress", "jamie@bungalow64.co.uk"),
                new SqlParameter("StartDate", DateTime.Parse("01-Mar-2020")),
                new SqlParameter("NumberOfHats", 14),
                new SqlParameter("Cost", 15.87));

            await TestRunner.ExecuteStoredProcedureNonQueryAsync("dbo.AddUser",
                new SqlParameter("FirstName", "Stuart"),
                new SqlParameter("LastName", "Burns"),
                new SqlParameter("EmailAddress", "stuart@bungalow64.co.uk"),
                new SqlParameter("StartDate", DateTime.Parse("01-Mar-2020")),
                new SqlParameter("NumberOfHats", 14),
                new SqlParameter("Cost", 15.87));

            QueryResult results = await TestRunner.ExecuteViewAsync("dbo.AllUsers");

            results
                .AssertRowCount(2)
                .AssertRowExists(new DataSetRow
                {
                    { "FirstName", "Jamie" }
                })
                .AssertRowExists(new DataSetRow
                {
                    { "FirstName", "Stuart" }
                });
        }

        [TestMethod]
        public async Task AllUsers_UseComplexData_ReuseTemplate()
        {
            UserTemplate user = new UserTemplate
            {
                { "FirstName", "Jamie" }
            };

            UserWithAddressTemplate userWithAddress = new UserWithAddressTemplate
            {
                User = user
            };
            await TestRunner.InsertTemplateAsync(userWithAddress);

            await TestRunner.InsertTemplateAsync(user);

            QueryResult results = await TestRunner.ExecuteViewAsync("dbo.AllUsers");

            results
                .AssertRowCount(1);
        }

        [TestMethod]
        public async Task AllUsers_UseFluent()
        {
            UserTemplate user = new UserTemplate()
                .WithId(1001)
                .WithFirstName("Jamie");

            UserWithAddressTemplate userWithAddress = new UserWithAddressTemplate
            {
                User = user
            };
            await TestRunner.InsertTemplateAsync(userWithAddress);
            await TestRunner.InsertTemplateAsync(user);

            QueryResult results = await TestRunner.ExecuteViewAsync("dbo.AllUsers");

            results
                .AssertRowCount(1)
                .AssertRowValues(0, new DataSetRow
                {
                    { "Id", 1001 },
                    { "FirstName", "Jamie" }
                });
        }

        [TestMethod]
        public async Task AllUsers_DefaultComplex()
        {
            UserWithAddressTemplate template = await TestRunner.InsertTemplateAsync<UserWithAddressTemplate>();

            QueryResult results = await TestRunner.ExecuteViewAsync("dbo.AllUsers");

            results
                .AssertRowCount(1)
                .AssertRowValues(0, new DataSetRow
                {
                    { "FirstName", template.User.DefaultData["FirstName"] }
                });
        }
    }
}

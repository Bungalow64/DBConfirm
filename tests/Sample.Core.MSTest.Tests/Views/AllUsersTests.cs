using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SQLConfirm.Core.DataResults;
using System.Threading.Tasks;
using Sample.Core.MSTest.Tests.Templates;
using Sample.Core.MSTest.Tests.Templates.Complex;
using SQLConfirm.Core.Data;
using SQLConfirm.Core.Parameters;
using SQLConfirm.Packages.SQLServer.MSTest;

namespace Sample.Core.MSTest.Tests.Views
{
    [TestClass]
    public class AllUsersTests : MSTestBase
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
                new SqlQueryParameter("FirstName", "Jamie"),
                new SqlQueryParameter("LastName", "Burns"),
                new SqlQueryParameter("EmailAddress", "jamie@bungalow64.co.uk"),
                new SqlQueryParameter("StartDate", DateTime.Parse("01-Mar-2020")),
                new SqlQueryParameter("NumberOfHats", 14),
                new SqlQueryParameter("Cost", 15.87));

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
                new SqlQueryParameter("FirstName", "Jamie"),
                new SqlQueryParameter("LastName", "Burns"),
                new SqlQueryParameter("EmailAddress", "jamie@bungalow64.co.uk"),
                new SqlQueryParameter("StartDate", DateTime.Parse("01-Mar-2020")),
                new SqlQueryParameter("NumberOfHats", 14),
                new SqlQueryParameter("Cost", 15.87));

            await TestRunner.ExecuteStoredProcedureNonQueryAsync("dbo.AddUser",
                new SqlQueryParameter("FirstName", "Stuart"),
                new SqlQueryParameter("LastName", "Burns"),
                new SqlQueryParameter("EmailAddress", "stuart@bungalow64.co.uk"),
                new SqlQueryParameter("StartDate", DateTime.Parse("01-Mar-2020")),
                new SqlQueryParameter("NumberOfHats", 14),
                new SqlQueryParameter("Cost", 15.87));

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

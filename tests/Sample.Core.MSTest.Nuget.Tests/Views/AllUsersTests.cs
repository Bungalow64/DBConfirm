using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using SQLConfirm.Packages.SQLServer.MSTest;
using SQLConfirm.Core.Parameters;
using SQLConfirm.Core.DataResults;
using SQLConfirm.Core.Data;

namespace Sample.Core.MSTest.Nuget.Tests.Views
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
    }
}

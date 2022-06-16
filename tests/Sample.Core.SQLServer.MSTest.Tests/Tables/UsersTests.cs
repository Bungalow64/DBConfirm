using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DBConfirm.Core.DataResults;
using System.Threading.Tasks;
using DBConfirm.Core.Data;
using DBConfirm.Core.Parameters;
using DBConfirm.Packages.SQLServer.MSTest;

namespace Sample.Core.SQLServer.MSTest.Tests.Tables
{
    [TestClass]
    public class UsersTests : MSTestBase
    {
        [TestMethod]
        public async Task Users_NoData_NothingReturned()
        {
            QueryResult results = await TestRunner.ExecuteTableAsync("dbo.Users");

            results
                .AssertRowCount(0);

            Assert.AreEqual(0, await TestRunner.CountRowsInTableAsync("dbo.Users"));
        }

        [TestMethod]
        public async Task Users_OneRow_OneUserReturned()
        {
            await TestRunner.ExecuteStoredProcedureNonQueryAsync("dbo.AddUser",
                new SqlQueryParameter("FirstName", "Jamie"),
                new SqlQueryParameter("LastName", "Burns"),
                new SqlQueryParameter("EmailAddress", "jamie@bungalow64.co.uk"),
                new SqlQueryParameter("StartDate", DateTime.Parse("01-Mar-2020")),
                new SqlQueryParameter("NumberOfHats", 14),
                new SqlQueryParameter("Cost", 15.87));

            QueryResult results = await TestRunner.ExecuteTableAsync("dbo.Users");

            results
                .AssertRowCount(1)
                .AssertRowValues(0, new DataSetRow
                {
                    { "FirstName", "Jamie" }
                });

            Assert.AreEqual(1, await TestRunner.CountRowsInTableAsync("dbo.Users"));
        }

        [TestMethod]
        public async Task Users_TwoRows_TwoUsersReturned()
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

            QueryResult results = await TestRunner.ExecuteTableAsync("dbo.Users");

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
        public async Task Users_OneRow_OneUserReturned_ByCommand()
        {
            await TestRunner.ExecuteStoredProcedureNonQueryAsync("dbo.AddUser",
                new SqlQueryParameter("FirstName", "Jamie"),
                new SqlQueryParameter("LastName", "Burns"),
                new SqlQueryParameter("EmailAddress", "jamie@bungalow64.co.uk"),
                new SqlQueryParameter("StartDate", DateTime.Parse("01-Mar-2020")),
                new SqlQueryParameter("NumberOfHats", 14),
                new SqlQueryParameter("Cost", 15.87));

            QueryResult results = await TestRunner.ExecuteCommandAsync("SELECT * FROM dbo.Users");

            results
                .AssertRowCount(1)
                .AssertRowValues(0, new DataSetRow
                {
                    { "FirstName", "Jamie" }
                });
        }
    }
}

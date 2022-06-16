using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DBConfirm.Core.DataResults;
using System.Threading.Tasks;
using DBConfirm.Core.Data;
using DBConfirm.Core.Parameters;
using DBConfirm.Packages.MySQL.MSTest;

namespace Sample.Core.MySQL.MSTest.Tests.Tables
{
    [TestClass]
    public class UsersTests : MSTestBase
    {
        [TestMethod]
        public async Task Users_NoData_NothingReturned()
        {
            QueryResult results = await TestRunner.ExecuteTableAsync("Users");

            results
                .AssertRowCount(0);

            Assert.AreEqual(0, await TestRunner.CountRowsInTableAsync("Users"));
        }

        [TestMethod]
        public async Task Users_OneRow_OneUserReturned()
        {
            await TestRunner.ExecuteStoredProcedureNonQueryAsync("AddUser",
                new SqlQueryParameter("p_FirstName", "Jamie"),
                new SqlQueryParameter("p_LastName", "Burns"),
                new SqlQueryParameter("p_EmailAddress", "jamie@bungalow64.co.uk"),
                new SqlQueryParameter("p_StartDate", DateTime.Parse("01-Mar-2020")),
                new SqlQueryParameter("p_NumberOfHats", 14),
                new SqlQueryParameter("p_Cost", 15.87));

            QueryResult results = await TestRunner.ExecuteTableAsync(".Users");

            results
                .AssertRowCount(1)
                .AssertRowValues(0, new DataSetRow
                {
                    { "FirstName", "Jamie" }
                });

            Assert.AreEqual(1, await TestRunner.CountRowsInTableAsync("Users"));
        }

        [TestMethod]
        public async Task Users_TwoRows_TwoUsersReturned()
        {
            await TestRunner.ExecuteStoredProcedureNonQueryAsync("AddUser",
                new SqlQueryParameter("p_FirstName", "Jamie"),
                new SqlQueryParameter("p_LastName", "Burns"),
                new SqlQueryParameter("p_EmailAddress", "jamie@bungalow64.co.uk"),
                new SqlQueryParameter("p_StartDate", DateTime.Parse("01-Mar-2020")),
                new SqlQueryParameter("p_NumberOfHats", 14),
                new SqlQueryParameter("p_Cost", 15.87));

            await TestRunner.ExecuteStoredProcedureNonQueryAsync("AddUser",
                new SqlQueryParameter("p_FirstName", "Stuart"),
                new SqlQueryParameter("p_LastName", "Burns"),
                new SqlQueryParameter("p_EmailAddress", "stuart@bungalow64.co.uk"),
                new SqlQueryParameter("p_StartDate", DateTime.Parse("01-Mar-2020")),
                new SqlQueryParameter("p_NumberOfHats", 14),
                new SqlQueryParameter("p_Cost", 15.87));

            QueryResult results = await TestRunner.ExecuteTableAsync("Users");

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
            await TestRunner.ExecuteStoredProcedureNonQueryAsync("AddUser",
                new SqlQueryParameter("p_FirstName", "Jamie"),
                new SqlQueryParameter("p_LastName", "Burns"),
                new SqlQueryParameter("p_EmailAddress", "jamie@bungalow64.co.uk"),
                new SqlQueryParameter("p_StartDate", DateTime.Parse("01-Mar-2020")),
                new SqlQueryParameter("p_NumberOfHats", 14),
                new SqlQueryParameter("p_Cost", 15.87));

            QueryResult results = await TestRunner.ExecuteCommandAsync("SELECT * FROM Users");

            results
                .AssertRowCount(1)
                .AssertRowValues(0, new DataSetRow
                {
                    { "FirstName", "Jamie" }
                });
        }
    }
}

using Common;
using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using System;
using System.Threading.Tasks;

namespace Sample.Core.MSTest.Tests.StoredProcedures
{
    [TestClass]
    public class GetUser : TestBase
    {
        [TestMethod]
        public async Task GetUser_NoData_ReturnNoRows()
        {
            QueryResult result = await ExecuteStoredProcedureQueryAsync("dbo.GetUser", new SqlParameter("EmailAddress", "jamie@bungalow64.co.uK"));

            result
                .AssertRowCount(0);
        }

        [TestMethod]
        public async Task GetUser_MatchEmailAddress_ReturnNames()
        {
            await ExecuteStoredProcedureNonQueryAsync("dbo.AddUser",
                new SqlParameter("FirstName", "Jamie"),
                new SqlParameter("LastName", "Burns"),
                new SqlParameter("EmailAddress", "jamie@bungalow64.co.uk"),
                new SqlParameter("StartDate", DateTime.Parse("01-Mar-2020")),
                new SqlParameter("NumberOfHats", 14),
                new SqlParameter("Cost", 15.87));

            QueryResult result = await ExecuteStoredProcedureQueryAsync("dbo.GetUser", new SqlParameter("EmailAddress", "jamie@bungalow64.co.uK"));

            result
                .AssertRowCount(1)
                .AssertColumnsExist("FirstName", "LastName")
                .AssertRowValues(0, new DataSetRow
                {
                    { "FirstName", "Jamie" },
                    { "LastName", "Burns" }
                });
        }
    }
}

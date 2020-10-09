using Common;
using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.Core.MSTest.Tests.StoredProcedures
{
    [TestClass]
    public class GetUserTests : TestBase
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

        [TestMethod]
        public async Task GetUser_MatchEmailAddress_ReturnNamesAndCount()
        {
            await ExecuteStoredProcedureNonQueryAsync("dbo.AddUser",
                new SqlParameter("FirstName", "Jamie"),
                new SqlParameter("LastName", "Burns"),
                new SqlParameter("EmailAddress", "jamie@bungalow64.co.uk"),
                new SqlParameter("StartDate", DateTime.Parse("01-Mar-2020")),
                new SqlParameter("NumberOfHats", 14),
                new SqlParameter("Cost", 15.87));

            IList<QueryResult> result = await ExecuteStoredProcedureMultipleDataSetAsync("dbo.GetUser", new SqlParameter("EmailAddress", "jamie@bungalow64.co.uK"));

            result[0]
                .AssertRowCount(1)
                .AssertColumnsExist("FirstName", "LastName")
                .AssertRowValues(0, new DataSetRow
                {
                    { "FirstName", "Jamie" },
                    { "LastName", "Burns" }
                });

            result[1]
                .AssertRowCount(1)
                .AssertColumnsExist("TotalUsers")
                .AssertValue(0, "TotalUsers", 1);
        }

        [TestMethod]
        public async Task GetUser_MatchEmailAddress_ReturnNamesAndCount_ByCommand()
        {
            await ExecuteCommandNoResultsAsync("EXEC dbo.AddUser @FirstName, @LastName, @EmailAddress, @StartDate, @NumberOfHats, @Cost",
                new SqlParameter("FirstName", "Jamie"),
                new SqlParameter("LastName", "Burns"),
                new SqlParameter("EmailAddress", "jamie@bungalow64.co.uk"),
                new SqlParameter("StartDate", DateTime.Parse("01-Mar-2020")),
                new SqlParameter("NumberOfHats", 14),
                new SqlParameter("Cost", 15.87));

            IList<QueryResult> result = await ExecuteCommandMultipleDataSetAsync("EXEC dbo.GetUser @EmailAddress", new SqlParameter("EmailAddress", "jamie@bungalow64.co.uK"));

            result[0]
                .AssertRowCount(1)
                .AssertColumnsExist("FirstName", "LastName")
                .AssertRowValues(0, new DataSetRow
                {
                    { "FirstName", "Jamie" },
                    { "LastName", "Burns" }
                });

            result[1]
                .AssertRowCount(1)
                .AssertColumnsExist("TotalUsers")
                .AssertValue(0, "TotalUsers", 1);
        }
    }
}

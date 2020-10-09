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
    public class CountUsersTests : TestBase
    {
        [TestMethod]
        public async Task CountUsers_NoData_ReturnNoRows()
        {
            QueryResult result = await ExecuteStoredProcedureQueryAsync("dbo.CountUsers", new SqlParameter("EmailAddress", "jamie@bungalow64.co.uK"));

            result
                .AssertRowCount(1);
        }

        [TestMethod]
        public async Task CountUsers_MatchEmailAddress_Return1()
        {
            await ExecuteStoredProcedureNonQueryAsync("dbo.AddUser",
                new SqlParameter("FirstName", "Jamie"),
                new SqlParameter("LastName", "Burns"),
                new SqlParameter("EmailAddress", "jamie@bungalow64.co.uk"),
                new SqlParameter("StartDate", DateTime.Parse("01-Mar-2020")),
                new SqlParameter("NumberOfHats", 14),
                new SqlParameter("Cost", 15.87));

            ScalarResult<int> result = await ExecuteStoredProcedureScalarAsync<int>("dbo.CountUsers", new SqlParameter("EmailAddress", "jamie@bungalow64.co.uK"));

            result
                .AssertValue(1);
        }

        [TestMethod]
        public async Task CountUsers_NoMatchEmailAddress_Return0()
        {
            await ExecuteStoredProcedureNonQueryAsync("dbo.AddUser",
                new SqlParameter("FirstName", "Jamie"),
                new SqlParameter("LastName", "Burns"),
                new SqlParameter("EmailAddress", "jamie@bungalow64.co.uk"),
                new SqlParameter("StartDate", DateTime.Parse("01-Mar-2020")),
                new SqlParameter("NumberOfHats", 14),
                new SqlParameter("Cost", 15.87));

            ScalarResult<int> result = await ExecuteStoredProcedureScalarAsync<int>("dbo.CountUsers", new SqlParameter("EmailAddress", "RARARAR@dsf.co.uK"));

            result
                .AssertValue(0);

            result
                .AssertValue(ExpectedData.IsNotNull());
        }

        [TestMethod]
        public async Task CountUsers_NoMatchEmailAddress_Return0_ByCommand()
        {
            await ExecuteStoredProcedureNonQueryAsync("dbo.AddUser",
                new SqlParameter("FirstName", "Jamie"),
                new SqlParameter("LastName", "Burns"),
                new SqlParameter("EmailAddress", "jamie@bungalow64.co.uk"),
                new SqlParameter("StartDate", DateTime.Parse("01-Mar-2020")),
                new SqlParameter("NumberOfHats", 14),
                new SqlParameter("Cost", 15.87));

            ScalarResult<int> result = await ExecuteCommandScalarAsync<int>("EXEC dbo.CountUsers @EmailAddress", new SqlParameter("EmailAddress", "jamie@bungalow64.co.uk"));

            result
                .AssertValue(1);
        }
    }
}

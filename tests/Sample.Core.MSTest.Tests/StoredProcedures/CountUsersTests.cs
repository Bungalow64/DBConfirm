using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sample.Core.MSTest.Tests.Templates;
using System;
using DBConfirm.Core.DataResults;
using System.Threading.Tasks;
using DBConfirm.Core.Data;
using DBConfirm.Core.Parameters;
using DBConfirm.Packages.SQLServer.MSTest;

namespace Sample.Core.MSTest.Tests.StoredProcedures
{
    [TestClass]
    public class CountUsersTests : MSTestBase
    {
        [TestMethod]
        public async Task CountUsers_NoData_ReturnNoRows()
        {
            QueryResult result = await TestRunner.ExecuteStoredProcedureQueryAsync("dbo.CountUsers", new SqlQueryParameter("EmailAddress", "jamie@bungalow64.co.uK"));

            result
                .AssertRowCount(1);
        }

        [TestMethod]
        public async Task CountUsers_MatchEmailAddress_Return1()
        {
            await TestRunner.ExecuteStoredProcedureNonQueryAsync("dbo.AddUser",
                new SqlQueryParameter("FirstName", "Jamie"),
                new SqlQueryParameter("LastName", "Burns"),
                new SqlQueryParameter("EmailAddress", "jamie@bungalow64.co.uk"),
                new SqlQueryParameter("StartDate", DateTime.Parse("01-Mar-2020")),
                new SqlQueryParameter("NumberOfHats", 14),
                new SqlQueryParameter("Cost", 15.87));

            ScalarResult<int> result = await TestRunner.ExecuteStoredProcedureScalarAsync<int>("dbo.CountUsers", new SqlQueryParameter("EmailAddress", "jamie@bungalow64.co.uK"));

            result
                .AssertValue(1);
        }

        [TestMethod]
        public async Task CountUsers_NoMatchEmailAddress_Return0()
        {
            await TestRunner.ExecuteStoredProcedureNonQueryAsync("dbo.AddUser",
                new SqlQueryParameter("FirstName", "Jamie"),
                new SqlQueryParameter("LastName", "Burns"),
                new SqlQueryParameter("EmailAddress", "jamie@bungalow64.co.uk"),
                new SqlQueryParameter("StartDate", DateTime.Parse("01-Mar-2020")),
                new SqlQueryParameter("NumberOfHats", 14),
                new SqlQueryParameter("Cost", 15.87));

            ScalarResult<int> result = await TestRunner.ExecuteStoredProcedureScalarAsync<int>("dbo.CountUsers", new SqlQueryParameter("EmailAddress", "RARARAR@dsf.co.uK"));

            result
                .AssertValue(0);

            result
                .AssertValue(Comparisons.IsNotNull());
        }

        [TestMethod]
        public async Task CountUsers_NoMatchEmailAddress_Return1_ByCommand()
        {
            await TestRunner.ExecuteStoredProcedureNonQueryAsync("dbo.AddUser",
                new SqlQueryParameter("FirstName", "Jamie"),
                new SqlQueryParameter("LastName", "Burns"),
                new SqlQueryParameter("EmailAddress", "jamie@bungalow64.co.uk"),
                new SqlQueryParameter("StartDate", DateTime.Parse("01-Mar-2020")),
                new SqlQueryParameter("NumberOfHats", 14),
                new SqlQueryParameter("Cost", 15.87));

            ScalarResult<int> result = await TestRunner.ExecuteCommandScalarAsync<int>("EXEC dbo.CountUsers @EmailAddress", new SqlQueryParameter("EmailAddress", "jamie@bungalow64.co.uk"));

            result
                .AssertValue(1);
        }

        [TestMethod]
        public async Task CountUsers_MatchEmailAddress_Return1_UseDataSetupIdentity()
        {
            await TestRunner.InsertDataAsync("dbo.Users", new DataSetRow
            {
                { "FirstName", "Jamie" },
                { "LastName", "Burns" },
                { "EmailAddress", "jamie@bungalow64.co.uk" },
                { "StartDate", DateTime.Parse("01-Mar-2020") },
                { "NumberOfHats", 14 },
                { "Cost", 15.87 },
                { "CreatedDate", DateTime.Parse("01-Feb-2020") },
            });

            ScalarResult<int> result = await TestRunner.ExecuteStoredProcedureScalarAsync<int>("dbo.CountUsers", new SqlQueryParameter("EmailAddress", "jamie@bungalow64.co.uK"));

            result
                .AssertValue(1);
        }

        [TestMethod]
        public async Task CountUsers_MatchEmailAddress_Return1_UseDefaultDataTemplateSetupIdentity()
        {
            await TestRunner.InsertTemplateAsync<UserTemplate>();

            ScalarResult<int> result = await TestRunner.ExecuteStoredProcedureScalarAsync<int>("dbo.CountUsers", new DataSetRow
            {
                { "EmailAddress", "jamie@bungalow64.co.uK" }
            });

            result
                .AssertValue(1);
        }

        [TestMethod]
        public async Task CountUsers_MatchEmailAddress_Return1_UseDataTemplateSetupIdentity()
        {
            await TestRunner.InsertTemplateAsync(new UserTemplate
            {
                { "EmailAddress", "jimmy@bungalow64.co.uk" }
            });

            ScalarResult<int> result = await TestRunner.ExecuteStoredProcedureScalarAsync<int>("dbo.CountUsers", new DataSetRow
            {
                { "EmailAddress", "jimmy@bungalow64.co.uK" }
            });

            result
                .AssertValue(1);
        }
    }
}

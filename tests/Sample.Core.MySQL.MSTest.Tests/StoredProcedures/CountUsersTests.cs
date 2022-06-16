using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sample.Core.MySQL.MSTest.Tests.Templates;
using System;
using DBConfirm.Core.DataResults;
using System.Threading.Tasks;
using DBConfirm.Core.Data;
using DBConfirm.Core.Parameters;
using DBConfirm.Packages.MySQL.MSTest;

namespace Sample.Core.MySQL.MSTest.Tests.StoredProcedures
{
    [TestClass]
    public class CountUsersTests : MSTestBase
    {
        [TestMethod]
        public async Task CountUsers_NoData_ReturnNoRows()
        {
            QueryResult result = await TestRunner.ExecuteStoredProcedureQueryAsync("CountUsers", new SqlQueryParameter("p_EmailAddress", "jamie@bungalow64.co.uK"));

            result
                .AssertRowCount(1);
        }

        [TestMethod]
        public async Task CountUsers_MatchEmailAddress_Return1()
        {
            await TestRunner.ExecuteStoredProcedureNonQueryAsync("AddUser",
                new SqlQueryParameter("p_FirstName", "Jamie"),
                new SqlQueryParameter("p_LastName", "Burns"),
                new SqlQueryParameter("p_EmailAddress", "jamie@bungalow64.co.uk"),
                new SqlQueryParameter("p_StartDate", DateTime.Parse("01-Mar-2020")),
                new SqlQueryParameter("p_NumberOfHats", 14),
                new SqlQueryParameter("p_Cost", 15.87));

            ScalarResult<long> result = await TestRunner.ExecuteStoredProcedureScalarAsync<long>("CountUsers", new SqlQueryParameter("p_EmailAddress", "jamie@bungalow64.co.uK"));

            result
                .AssertValue(1L);
        }

        [TestMethod]
        public async Task CountUsers_NoMatchEmailAddress_Return0()
        {
            await TestRunner.ExecuteStoredProcedureNonQueryAsync("AddUser",
                new SqlQueryParameter("p_FirstName", "Jamie"),
                new SqlQueryParameter("p_LastName", "Burns"),
                new SqlQueryParameter("p_EmailAddress", "jamie@bungalow64.co.uk"),
                new SqlQueryParameter("p_StartDate", DateTime.Parse("01-Mar-2020")),
                new SqlQueryParameter("p_NumberOfHats", 14),
                new SqlQueryParameter("p_Cost", 15.87));

            ScalarResult<long> result = await TestRunner.ExecuteStoredProcedureScalarAsync<long>("CountUsers", new SqlQueryParameter("p_EmailAddress", "RARARAR@dsf.co.uK"));

            result
                .AssertValue(0L);

            result
                .AssertValue(Comparisons.IsNotNull());
        }

        [TestMethod]
        public async Task CountUsers_NoMatchEmailAddress_Return1_ByCommand()
        {
            await TestRunner.ExecuteStoredProcedureNonQueryAsync("AddUser",
                new SqlQueryParameter("p_FirstName", "Jamie"),
                new SqlQueryParameter("p_LastName", "Burns"),
                new SqlQueryParameter("p_EmailAddress", "jamie@bungalow64.co.uk"),
                new SqlQueryParameter("p_StartDate", DateTime.Parse("01-Mar-2020")),
                new SqlQueryParameter("p_NumberOfHats", 14),
                new SqlQueryParameter("p_Cost", 15.87));

            ScalarResult<long> result = await TestRunner.ExecuteCommandScalarAsync<long>("CALL CountUsers (@EmailAddress)", new SqlQueryParameter("EmailAddress", "jamie@bungalow64.co.uk"));

            result
                .AssertValue(1L);
        }

        [TestMethod]
        public async Task CountUsers_MatchEmailAddress_Return1_UseDataSetupIdentity()
        {
            await TestRunner.InsertDataAsync("Users", new DataSetRow
            {
                { "FirstName", "Jamie" },
                { "LastName", "Burns" },
                { "EmailAddress", "jamie@bungalow64.co.uk" },
                { "StartDate", DateTime.Parse("01-Mar-2020") },
                { "NumberOfHats", 14 },
                { "Cost", 15.87 },
                { "CreatedDate", DateTime.Parse("01-Feb-2020") },
            });

            ScalarResult<long> result = await TestRunner.ExecuteStoredProcedureScalarAsync<long>("CountUsers", new SqlQueryParameter("p_EmailAddress", "jamie@bungalow64.co.uK"));

            result
                .AssertValue(1L);
        }

        [TestMethod]
        public async Task CountUsers_MatchEmailAddress_Return1_UseDefaultDataTemplateSetupIdentity()
        {
            await TestRunner.InsertTemplateAsync<UserTemplate>();

            ScalarResult<long> result = await TestRunner.ExecuteStoredProcedureScalarAsync<long>("CountUsers", new DataSetRow
            {
                { "p_EmailAddress", "jamie@bungalow64.co.uK" }
            });

            result
                .AssertValue(1L);
        }

        [TestMethod]
        public async Task CountUsers_MatchEmailAddress_Return1_UseDataTemplateSetupIdentity()
        {
            await TestRunner.InsertTemplateAsync(new UserTemplate
            {
                { "EmailAddress", "jimmy@bungalow64.co.uk" }
            });

            ScalarResult<long> result = await TestRunner.ExecuteStoredProcedureScalarAsync<long>("CountUsers", new DataSetRow
            {
                { "p_EmailAddress", "jimmy@bungalow64.co.uK" }
            });

            result
                .AssertValue(1L);
        }
    }
}

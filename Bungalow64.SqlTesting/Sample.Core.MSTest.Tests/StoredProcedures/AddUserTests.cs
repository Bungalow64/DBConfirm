using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using SQLConfirm.Core.DataResults;
using SQLConfirm.Core.Comparisons;
using SQLConfirm.Core.Data;
using SQLConfirm.Frameworks.MSTest2;
using SQLConfirm.Core.Parameters;

namespace Sample.Core.MSTest.Tests.StoredProcedures
{
    [TestClass]
    public class AddUserTests : TestBase
    {
        [TestMethod]
        public async Task AddUser_ValidData_UserAdded()
        {
            var expectedData = new DataSetRow
            {
                ["FirstName"] = "Jamie",
                ["LastName"] = ExpectedData.NotMatchesRegex(".*@.*"),
                ["EmailAddress"] = ExpectedData.MatchesRegex(".*@.*"),
                ["CreatedDate"] = ExpectedData.IsUtcNow(),
                ["StartDate"] = ExpectedData.IsDay("01-Mar-2020 00:10:00"),
                ["IsActive"] = true,
                ["NumberOfHats"] = 14L,
                ["HatType"] = null,
                ["Cost"] = 15.87m
            };

            await TestRunner.ExecuteStoredProcedureNonQueryAsync("dbo.AddUser",
                new SqlQueryParameter("FirstName", "Jamie"),
                new SqlQueryParameter("LastName", "Burns"),
                new SqlQueryParameter("EmailAddress", "jamie@bungalow64.co.uk"),
                new SqlQueryParameter("StartDate", DateTime.Parse("01-Mar-2020")),
                new SqlQueryParameter("NumberOfHats", 14),
                new SqlQueryParameter("Cost", 15.87));

            await TestRunner.ExecuteStoredProcedureNonQueryAsync("dbo.AddUser",
                new SqlQueryParameter("FirstName", "AAA"),
                new SqlQueryParameter("LastName", "FFF"),
                new SqlQueryParameter("EmailAddress", "AAA@FFF.co.uk"),
                new SqlQueryParameter("StartDate", DateTime.Parse("01-Jan-2020")),
                new SqlQueryParameter("NumberOfHats", 3),
                new SqlQueryParameter("Cost", 34));

            QueryResult data = await TestRunner.ExecuteTableAsync("dbo.Users");

            data.AssertRowCount(2);
            data.AssertColumnsExist("FirstName", "LastName", "EmailAddress", "CreatedDate");
            data.AssertColumnsNotExist("Age", "JobTitle");
            data.AssertValue(0, "FirstName", "Jamie");

            data
                .ValidateRow(0)
                .AssertValues(expectedData);

            data
                .AssertRowValues(0, expectedData);

            data
                .ValidateRow(0)
                .AssertValue("FirstName", "Jamie");

            data
                .ValidateRow(0)
                .AssertValue("FirstName", ExpectedData.HasLength(5));

            data
                .AssertRowValues(0, new DataSetRow
                {
                    { "FirstName", "Jamie" },
                    { "LastName", "Burns" }
                })
                .AssertRowValues(1, new DataSetRow
                {
                    { "FirstName", "AAA" },
                    { "LastName", "FFF" }
                });

            data
                .AssertRowExists(new DataSetRow
                {
                    { "FirstName", "Jamie" },
                    { "LastName", "Burns" }
                });

            data
                .AssertRowDoesNotExist(new DataSetRow
                {
                    { "FirstName", "Jeff" },
                    { "LastName", "Burns" }
                });
        }
    }
}

using Common;
using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using System;
using System.Threading.Tasks;

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
                ["LastName"] = ExpectedData.IsNotNull(),
                ["EmailAddress"] = "jamie@bungalow64.co.uk",
                ["CreatedDate"] = ExpectedData.IsUtcNow(),
                ["StartDate"] = ExpectedData.IsDay("01-Mar-2020 00:10:00"),
                ["IsActive"] = true,
                ["NumberOfHats"] = 14L,
                ["HatType"] = null,
                ["Cost"] = 15.87m
            };

            await ExecuteStoredProcedureAsync("dbo.AddUser",
                new SqlParameter("FirstName", "Jamie"),
                new SqlParameter("LastName", "Burns"),
                new SqlParameter("EmailAddress", "jamie@bungalow64.co.uk"),
                new SqlParameter("StartDate", DateTime.Parse("01-Mar-2020")),
                new SqlParameter("NumberOfHats", 14),
                new SqlParameter("Cost", 15.87));

            QueryResult data = GetAllRows("dbo.Users");

            data.AssertRowCount(1);
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
        }
    }
}

using System;
using System.Threading.Tasks;
using DBConfirm.Core.DataResults;
using DBConfirm.Core.Data;
using DBConfirm.Core.Parameters;
using DBConfirm.Packages.SQLServer.NUnit;
using NUnit.Framework;

namespace Sample.Core.NUnit.Tests.StoredProcedures
{
    public class AddUserTests : NUnitBase
    {
        [Test]
        public async Task AddUser_ValidData_UserAdded()
        {
            var expectedData = new DataSetRow
            {
                ["FirstName"] = "Jamie",
                ["LastName"] = Comparisons.NotMatchesRegex(".*@.*"),
                ["EmailAddress"] = Comparisons.MatchesRegex(".*@.*"),
                ["CreatedDate"] = Comparisons.IsUtcNow(),
                ["StartDate"] = Comparisons.IsDay("01-Mar-2020 00:10:00"),
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
                .AssertValue("FirstName", Comparisons.HasLength(5));

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

        [Test]
        public async Task AddUser_Incorrect_AssertRowCount_TestFailure()
        {
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

            var exception = Assert.Throws<AssertionException>(() => data.AssertRowCount(3));

            Assert.AreEqual("  The total row count is unexpected\r\n  Expected: 3\r\n  But was:  2\r\n", exception.Message);
        }

        [Test]
        public async Task AddUser_Incorrect_AssertRowDoesNotExist_TestFailure()
        {
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

            var exception = Assert.Throws<AssertionException>(() => data.AssertRowDoesNotExist(new DataSetRow
                {
                    { "FirstName", "Jamie" },
                    { "LastName", "Burns" }
                }));

            Assert.AreEqual("Row 0 matches the expected data that should not match anything: \r\n[FirstName, Jamie]\r\n[LastName, Burns]", exception.Message);
        }

        [Test]
        public async Task AddUser_Incorrect_AssertRowDoesNotExist_SecondRow_TestFailure()
        {
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

            var exception = Assert.Throws<AssertionException>(() => data.AssertRowDoesNotExist(new DataSetRow
                {
                    { "FirstName", "AAA" },
                    { "LastName", "FFF" }
                }));

            Assert.AreEqual("Row 1 matches the expected data that should not match anything: \r\n[FirstName, AAA]\r\n[LastName, FFF]", exception.Message);
        }
    }
}

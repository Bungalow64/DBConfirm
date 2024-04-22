using DBConfirm.Core.Data;
using DBConfirm.Core.DataResults;
using DBConfirm.Core.Parameters;
using DBConfirm.Packages.SQLServer.NUnit;
using Microsoft.Data.SqlClient;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Core.NUnit.Tests.StoredProcedures;

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

        Assert.AreEqual($"  The total row count is unexpected{Environment.NewLine}  Expected: 3{Environment.NewLine}  But was:  2{Environment.NewLine}", exception.Message);
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

        Assert.AreEqual($"Row 0 matches the expected data that should not match anything: {Environment.NewLine}[FirstName, Jamie]{Environment.NewLine}[LastName, Burns]", exception.Message);
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

        Assert.AreEqual($"Row 1 matches the expected data that should not match anything: {Environment.NewLine}[FirstName, AAA]{Environment.NewLine}[LastName, FFF]", exception.Message);
    }

    [TestCase(true, true)]
    [TestCase(false, true)]
    [TestCase(true, false)]
    [TestCase(false, false)]
    public async Task AddUser_NullRequiredParameter_ExpectError(bool useProcedure, bool useSqlParameters)
    {
        SqlQueryParameter[] parameters =
        [
            new("FirstName", null),
            new("LastName", "FFF"),
            new("EmailAddress", "AAA@FFF.co.uk"),
            new("StartDate", DateTime.Parse("01-Jan-2020")),
            new("NumberOfHats", 3),
            new("Cost", 34)
        ];

        Dictionary<string, object> dictionary = parameters.ToDictionary(p => p.ParameterName, p => p.Value);

        ErrorResult error;
        if (useProcedure)
        {
            if (useSqlParameters)
            {
                error = await TestRunner.ExecuteStoredProcedureErrorAsync("dbo.AddUser", parameters);
            }
            else
            {
                error = await TestRunner.ExecuteStoredProcedureErrorAsync("dbo.AddUser", dictionary);
            }
        }
        else
        {
            if (useSqlParameters)
            {
                error = await TestRunner.ExecuteCommandErrorAsync("EXEC dbo.AddUser @FirstName, @LastName, @EmailAddress, @StartDate, @NumberOfHats, @Cost", parameters);
            }
            else
            {
                error = await TestRunner.ExecuteCommandErrorAsync("EXEC dbo.AddUser @FirstName, @LastName, @EmailAddress, @StartDate, @NumberOfHats, @Cost", dictionary);
            }
        }

        error.AssertError();
        error.AssertType(typeof(SqlException));
        error.AssertMessage(Comparisons.StartsWith("Cannot insert the value NULL into column 'FirstName', table 'SampleDB.dbo.Users'; column does not allow nulls."));
    }

    [TestCase(true, true)]
    [TestCase(false, true)]
    [TestCase(true, false)]
    [TestCase(false, false)]
    public async Task AddUser_ValidRequest_AssertError_ShouldFailTest(bool useProcedure, bool useSqlParameters)
    {
        using (new TestExecutionContext.IsolatedContext())
        {
            SqlQueryParameter[] parameters =
            [
                new("FirstName", "AAA"),
                new("LastName", "FFF"),
                new("EmailAddress", "AAA@FFF.co.uk"),
                new("StartDate", DateTime.Parse("01-Jan-2020")),
                new("NumberOfHats", 3),
                new("Cost", 34)
            ];

            Dictionary<string, object> dictionary = parameters.ToDictionary(p => p.ParameterName, p => p.Value);

            ErrorResult error;
            if (useProcedure)
            {
                if (useSqlParameters)
                {
                    error = await TestRunner.ExecuteStoredProcedureErrorAsync("dbo.AddUser", parameters);
                }
                else
                {
                    error = await TestRunner.ExecuteStoredProcedureErrorAsync("dbo.AddUser", dictionary);
                }
            }
            else
            {
                if (useSqlParameters)
                {
                    error = await TestRunner.ExecuteCommandErrorAsync("EXEC dbo.AddUser @FirstName, @LastName, @EmailAddress, @StartDate, @NumberOfHats, @Cost", parameters);
                }
                else
                {
                    error = await TestRunner.ExecuteCommandErrorAsync("EXEC dbo.AddUser @FirstName, @LastName, @EmailAddress, @StartDate, @NumberOfHats, @Cost", dictionary);
                }
            }

            try
            {
                error.AssertError();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("No error was found", ex.Message);
                return;
            }

            Assert.Fail("Expected test to fail, but it passed");
        }
    }

    [TestCase(true, true)]
    [TestCase(false, true)]
    [TestCase(true, false)]
    [TestCase(false, false)]
    public async Task AddUser_ValidRequest_AssertErrorType_ShouldFailTest(bool useProcedure, bool useSqlParameters)
    {
        using (new TestExecutionContext.IsolatedContext())
        {
            SqlQueryParameter[] parameters =
            [
                new("FirstName", "AAA"),
                new("LastName", "FFF"),
                new("EmailAddress", "AAA@FFF.co.uk"),
                new("StartDate", DateTime.Parse("01-Jan-2020")),
                new("NumberOfHats", 3),
                new("Cost", 34)
            ];

            Dictionary<string, object> dictionary = parameters.ToDictionary(p => p.ParameterName, p => p.Value);

            ErrorResult error;
            if (useProcedure)
            {
                if (useSqlParameters)
                {
                    error = await TestRunner.ExecuteStoredProcedureErrorAsync("dbo.AddUser", parameters);
                }
                else
                {
                    error = await TestRunner.ExecuteStoredProcedureErrorAsync("dbo.AddUser", dictionary);
                }
            }
            else
            {
                if (useSqlParameters)
                {
                    error = await TestRunner.ExecuteCommandErrorAsync("EXEC dbo.AddUser @FirstName, @LastName, @EmailAddress, @StartDate, @NumberOfHats, @Cost", parameters);
                }
                else
                {
                    error = await TestRunner.ExecuteCommandErrorAsync("EXEC dbo.AddUser @FirstName, @LastName, @EmailAddress, @StartDate, @NumberOfHats, @Cost", dictionary);
                }
            }

            try
            {
                error.AssertType(typeof(SqlException));
            }
            catch (Exception ex)
            {
                Assert.AreEqual("No error was found", ex.Message);
                return;
            }

            Assert.Fail("Expected test to fail, but it passed");
        }
    }

    [TestCase(true, true)]
    [TestCase(false, true)]
    [TestCase(true, false)]
    [TestCase(false, false)]
    public async Task AddUser_ValidRequest_AssertErrorMessage_ShouldFailTest(bool useProcedure, bool useSqlParameters)
    {
        using (new TestExecutionContext.IsolatedContext())
        {
            SqlQueryParameter[] parameters =
            [
                new("FirstName", "AAA"),
                new("LastName", "FFF"),
                new("EmailAddress", "AAA@FFF.co.uk"),
                new("StartDate", DateTime.Parse("01-Jan-2020")),
                new("NumberOfHats", 3),
                new("Cost", 34)
            ];

            Dictionary<string, object> dictionary = parameters.ToDictionary(p => p.ParameterName, p => p.Value);

            ErrorResult error;
            if (useProcedure)
            {
                if (useSqlParameters)
                {
                    error = await TestRunner.ExecuteStoredProcedureErrorAsync("dbo.AddUser", parameters);
                }
                else
                {
                    error = await TestRunner.ExecuteStoredProcedureErrorAsync("dbo.AddUser", dictionary);
                }
            }
            else
            {
                if (useSqlParameters)
                {
                    error = await TestRunner.ExecuteCommandErrorAsync("EXEC dbo.AddUser @FirstName, @LastName, @EmailAddress, @StartDate, @NumberOfHats, @Cost", parameters);
                }
                else
                {
                    error = await TestRunner.ExecuteCommandErrorAsync("EXEC dbo.AddUser @FirstName, @LastName, @EmailAddress, @StartDate, @NumberOfHats, @Cost", dictionary);
                }
            }

            try
            {
                error.AssertMessage(Comparisons.StartsWith("Cannot insert the value NULL into column 'FirstName', table 'SampleDB.dbo.Users'; column does not allow nulls."));
            }
            catch (Exception ex)
            {
                Assert.AreEqual("No error was found", ex.Message);
                return;
            }

            Assert.Fail("Expected test to fail, but it passed");
        }
    }

    [TestCase(true, true)]
    [TestCase(false, true)]
    [TestCase(true, false)]
    [TestCase(false, false)]
    public async Task AddUser_InvalidRequest_ExpectIncorrectMessage_AssertErrorMessage_ShouldFailTest(bool useProcedure, bool useSqlParameters)
    {
        using (new TestExecutionContext.IsolatedContext())
        {
            SqlQueryParameter[] parameters =
            [
                new("FirstName", null),
                new("LastName", "FFF"),
                new("EmailAddress", "AAA@FFF.co.uk"),
                new("StartDate", DateTime.Parse("01-Jan-2020")),
                new("NumberOfHats", 3),
                new("Cost", 34)
            ];

            Dictionary<string, object> dictionary = parameters.ToDictionary(p => p.ParameterName, p => p.Value);

            ErrorResult error;
            if (useProcedure)
            {
                if (useSqlParameters)
                {
                    error = await TestRunner.ExecuteStoredProcedureErrorAsync("dbo.AddUser", parameters);
                }
                else
                {
                    error = await TestRunner.ExecuteStoredProcedureErrorAsync("dbo.AddUser", dictionary);
                }
            }
            else
            {
                if (useSqlParameters)
                {
                    error = await TestRunner.ExecuteCommandErrorAsync("EXEC dbo.AddUser @FirstName, @LastName, @EmailAddress, @StartDate, @NumberOfHats, @Cost", parameters);
                }
                else
                {
                    error = await TestRunner.ExecuteCommandErrorAsync("EXEC dbo.AddUser @FirstName, @LastName, @EmailAddress, @StartDate, @NumberOfHats, @Cost", dictionary);
                }
            }

            try
            {
                error.AssertMessage(Comparisons.StartsWith("Cannot insert the value NULL into column 'LastName', table 'SampleDB.dbo.Users'; column does not allow nulls."));
            }
            catch (Exception ex)
            {
                Assert.AreEqual($"  Error result does not start with the expected string{Environment.NewLine}  Expected: String starting with \"Cannot insert the value NULL into column 'LastName', table 'SampleDB.dbo.Users'; column does not allow nulls.\"{Environment.NewLine}  But was:  \"Cannot insert the value NULL into column 'FirstName', table 'SampleDB.dbo.Users'; column does not allow nulls. INSERT fails.{Environment.NewLine}The statement has been terminated.\"{Environment.NewLine}", ex.Message);
                return;
            }

            Assert.Fail("Expected test to fail, but it passed");
        }
    }

    [TestCase(true, true)]
    [TestCase(false, true)]
    [TestCase(true, false)]
    [TestCase(false, false)]
    public async Task AddUser_InvalidRequest_ExpectIncorrectType_AssertErrorMessage_ShouldFailTest(bool useProcedure, bool useSqlParameters)
    {
        using (new TestExecutionContext.IsolatedContext())
        {
            SqlQueryParameter[] parameters =
            [
                new("FirstName", null),
                new("LastName", "FFF"),
                new("EmailAddress", "AAA@FFF.co.uk"),
                new("StartDate", DateTime.Parse("01-Jan-2020")),
                new("NumberOfHats", 3),
                new("Cost", 34)
            ];

            Dictionary<string, object> dictionary = parameters.ToDictionary(p => p.ParameterName, p => p.Value);

            ErrorResult error;
            if (useProcedure)
            {
                if (useSqlParameters)
                {
                    error = await TestRunner.ExecuteStoredProcedureErrorAsync("dbo.AddUser", parameters);
                }
                else
                {
                    error = await TestRunner.ExecuteStoredProcedureErrorAsync("dbo.AddUser", dictionary);
                }
            }
            else
            {
                if (useSqlParameters)
                {
                    error = await TestRunner.ExecuteCommandErrorAsync("EXEC dbo.AddUser @FirstName, @LastName, @EmailAddress, @StartDate, @NumberOfHats, @Cost", parameters);
                }
                else
                {
                    error = await TestRunner.ExecuteCommandErrorAsync("EXEC dbo.AddUser @FirstName, @LastName, @EmailAddress, @StartDate, @NumberOfHats, @Cost", dictionary);
                }
            }

            try
            {
                error.AssertType(typeof(NullReferenceException));
            }
            catch (Exception ex)
            {
                Assert.AreEqual($"  Error result has an unexpected value{Environment.NewLine}  Expected: <System.NullReferenceException>{Environment.NewLine}  But was:  <Microsoft.Data.SqlClient.SqlException>{Environment.NewLine}", ex.Message);
                return;
            }

            Assert.Fail("Expected test to fail, but it passed");
        }
    }
}

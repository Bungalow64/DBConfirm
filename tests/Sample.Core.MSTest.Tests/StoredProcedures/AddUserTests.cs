using DBConfirm.Core.Data;
using DBConfirm.Core.DataResults;
using DBConfirm.Core.Parameters;
using DBConfirm.Packages.SQLServer.MSTest;
using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Core.MSTest.Tests.StoredProcedures;

[TestClass]
public class AddUserTests : MSTestBase
{
    [TestMethod]
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

    [TestMethod]
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

        var exception = Assert.ThrowsException<AssertFailedException>(() => data.AssertRowCount(3));

        Assert.AreEqual("Assert.AreEqual failed. Expected:<3>. Actual:<2>. The total row count is unexpected", exception.Message);
    }

    [TestMethod]
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

        var exception = Assert.ThrowsException<AssertFailedException>(() => data.AssertRowDoesNotExist(new DataSetRow
            {
                { "FirstName", "Jamie" },
                { "LastName", "Burns" }
            }));

        Assert.AreEqual($"Assert.Fail failed. Row 0 matches the expected data that should not match anything: {Environment.NewLine}[FirstName, Jamie]{Environment.NewLine}[LastName, Burns]", exception.Message);
    }

    [TestMethod]
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

        var exception = Assert.ThrowsException<AssertFailedException>(() => data.AssertRowDoesNotExist(new DataSetRow
            {
                { "FirstName", "AAA" },
                { "LastName", "FFF" }
            }));

        Assert.AreEqual($"Assert.Fail failed. Row 1 matches the expected data that should not match anything: {Environment.NewLine}[FirstName, AAA]{Environment.NewLine}[LastName, FFF]", exception.Message);
    }

    [DataTestMethod]
    [DataRow(true, true)]
    [DataRow(false, true)]
    [DataRow(true, false)]
    [DataRow(false, false)]
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

    [DataTestMethod]
    [DataRow(true, true)]
    [DataRow(false, true)]
    [DataRow(true, false)]
    [DataRow(false, false)]
    public async Task AddUser_ValidRequest_AssertError_ShouldFailTest(bool useProcedure, bool useSqlParameters)
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
            Assert.AreEqual("Assert.Fail failed. No error was found", ex.Message);
            return;
        }

        Assert.Fail("Expected test to fail, but it passed");
    }

    [DataTestMethod]
    [DataRow(true, true)]
    [DataRow(false, true)]
    [DataRow(true, false)]
    [DataRow(false, false)]
    public async Task AddUser_ValidRequest_AssertErrorType_ShouldFailTest(bool useProcedure, bool useSqlParameters)
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
            Assert.AreEqual("Assert.Fail failed. No error was found", ex.Message);
            return;
        }

        Assert.Fail("Expected test to fail, but it passed");
    }

    [DataTestMethod]
    [DataRow(true, true)]
    [DataRow(false, true)]
    [DataRow(true, false)]
    [DataRow(false, false)]
    public async Task AddUser_ValidRequest_AssertErrorMessage_ShouldFailTest(bool useProcedure, bool useSqlParameters)
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
            Assert.AreEqual("Assert.Fail failed. No error was found", ex.Message);
            return;
        }

        Assert.Fail("Expected test to fail, but it passed");
    }

    [DataTestMethod]
    [DataRow(true, true)]
    [DataRow(false, true)]
    [DataRow(true, false)]
    [DataRow(false, false)]
    public async Task AddUser_InvalidRequest_ExpectIncorrectMessage_AssertErrorMessage_ShouldFailTest(bool useProcedure, bool useSqlParameters)
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
            Assert.AreEqual($$"""
StringAssert.StartsWith failed. String 'Cannot insert the value NULL into column 'FirstName', table 'SampleDB.dbo.Users'; column does not allow nulls. INSERT fails.
The statement has been terminated.' does not start with string 'Cannot insert the value NULL into column 'LastName', table 'SampleDB.dbo.Users'; column does not allow nulls.'. Error result does not start with the expected string.
""", ex.Message);
            return;
        }

        Assert.Fail("Expected test to fail, but it passed");
    }

    [DataTestMethod]
    [DataRow(true, true)]
    [DataRow(false, true)]
    [DataRow(true, false)]
    [DataRow(false, false)]
    public async Task AddUser_InvalidRequest_ExpectIncorrectType_AssertErrorMessage_ShouldFailTest(bool useProcedure, bool useSqlParameters)
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
            Assert.AreEqual("Assert.AreEqual failed. Expected:<System.NullReferenceException>. Actual:<Microsoft.Data.SqlClient.SqlException>. Error result has an unexpected value", ex.Message);
            return;
        }

        Assert.Fail("Expected test to fail, but it passed");
    }

    [TestMethod]
    public async Task AddUser_ValidData_AssertDifferentColumn_ShouldError()
    {
        try
        {
            await TestRunner.ExecuteStoredProcedureNonQueryAsync("dbo.AddUser",
                new SqlQueryParameter("FirstName", "Jamie"),
                new SqlQueryParameter("LastName", "Burns"),
                new SqlQueryParameter("EmailAddress", "jamie@bungalow64.co.uk"),
                new SqlQueryParameter("StartDate", DateTime.Parse("01-Mar-2020")),
                new SqlQueryParameter("NumberOfHats", 14),
                new SqlQueryParameter("Cost", 15.87));

            QueryResult data = await TestRunner.ExecuteTableAsync("dbo.Users");

            data.AssertColumnsExist("HatSize");
        }
        catch (Exception ex)
        {
            Assert.AreEqual("CollectionAssert.Contains failed. Expected column HatSize to be found but the only columns found are Id, FirstName, LastName, EmailAddress, CreatedDate, StartDate, IsActive, NumberOfHats, HatType, Cost", ex.Message);
            return;
        }

        Assert.Fail("Expected test to fail, but it passed");
    }

    [TestMethod]
    public async Task AddUser_ValidData_AssertDifferentData_ShouldError()
    {
        try
        {
            var expectedData = new DataSetRow
            {
                ["FirstName"] = "Ian",
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

            QueryResult data = await TestRunner.ExecuteTableAsync("dbo.Users");

            data.AssertRowCount(1);

            data
                .ValidateRow(0)
                .AssertValues(expectedData);
        }
        catch (Exception ex)
        {
            Assert.AreEqual("Assert.AreEqual failed. Expected:<Ian>. Actual:<Jamie>. Column FirstName in row 0 has an unexpected value", ex.Message);
            return;
        }

        Assert.Fail("Expected test to fail, but it passed");
    }

    [TestMethod]
    public async Task AddUser_ValidData_AssertWrongColumnNotExists_ShouldError()
    {
        try
        {
            await TestRunner.ExecuteStoredProcedureNonQueryAsync("dbo.AddUser",
                new SqlQueryParameter("FirstName", "Jamie"),
                new SqlQueryParameter("LastName", "Burns"),
                new SqlQueryParameter("EmailAddress", "jamie@bungalow64.co.uk"),
                new SqlQueryParameter("StartDate", DateTime.Parse("01-Mar-2020")),
                new SqlQueryParameter("NumberOfHats", 14),
                new SqlQueryParameter("Cost", 15.87));

            QueryResult data = await TestRunner.ExecuteTableAsync("dbo.Users");

            data.AssertColumnsNotExist("FirstName");
        }
        catch (Exception ex)
        {
            Assert.AreEqual("CollectionAssert.DoesNotContain failed. Expected column FirstName to not be found but it was found", ex.Message);
            return;
        }

        Assert.Fail("Expected test to fail, but it passed");
    }

    [TestMethod]
    public async Task AddUser_ValidData_AssertDifferentRowCount_ShouldError()
    {
        try
        {
            await TestRunner.ExecuteStoredProcedureNonQueryAsync("dbo.AddUser",
                new SqlQueryParameter("FirstName", "Jamie"),
                new SqlQueryParameter("LastName", "Burns"),
                new SqlQueryParameter("EmailAddress", "jamie@bungalow64.co.uk"),
                new SqlQueryParameter("StartDate", DateTime.Parse("01-Mar-2020")),
                new SqlQueryParameter("NumberOfHats", 14),
                new SqlQueryParameter("Cost", 15.87));

            QueryResult data = await TestRunner.ExecuteTableAsync("dbo.Users");

            data.AssertRowCount(2);
        }
        catch (Exception ex)
        {
            Assert.AreEqual("Assert.AreEqual failed. Expected:<2>. Actual:<1>. The total row count is unexpected", ex.Message);
            return;
        }

        Assert.Fail("Expected test to fail, but it passed");
    }

    [TestMethod]
    public async Task AddUser_ValidData_AssertDifferentValue_ShouldError()
    {
        try
        {
            await TestRunner.ExecuteStoredProcedureNonQueryAsync("dbo.AddUser",
                new SqlQueryParameter("FirstName", "Jamie"),
                new SqlQueryParameter("LastName", "Burns"),
                new SqlQueryParameter("EmailAddress", "jamie@bungalow64.co.uk"),
                new SqlQueryParameter("StartDate", DateTime.Parse("01-Mar-2020")),
                new SqlQueryParameter("NumberOfHats", 14),
                new SqlQueryParameter("Cost", 15.87));

            QueryResult data = await TestRunner.ExecuteTableAsync("dbo.Users");

            data.AssertRowCount(1);
            data.AssertValue(0, "FirstName", "Jamie2");
        }
        catch (Exception ex)
        {
            Assert.AreEqual("Assert.AreEqual failed. Expected:<Jamie2>. Actual:<Jamie>. Column FirstName in row 0 has an unexpected value", ex.Message);
            return;
        }

        Assert.Fail("Expected test to fail, but it passed");
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using DBConfirm.Packages.SQLServer.MSTest;
using Microsoft.Data.SqlClient;
using System;

namespace Sample.Core.MSTest.Nuget.Tests.Tables;

[TestClass]
public class InvalidTableTests : MSTestBase
{
    #region Can Query

    [TestMethod]
    public async Task InvalidTable_Query_ReturnError()
    {
        SqlException exception = await Assert.ThrowsExceptionAsync<SqlException>(async () => await TestRunner.ExecuteTableAsync("dbo.UnknownTable"));

        Assert.AreEqual("Invalid object name 'dbo.UnknownTable'.", exception.Message);
    }

    [TestMethod]
    public async Task InvalidTableWithUnescapedSpecialCharacter_Query_ReturnError()
    {
        SqlException exception = await Assert.ThrowsExceptionAsync<SqlException>(async () => await TestRunner.ExecuteTableAsync("dbo.UnknownT'able"));

        Assert.AreEqual("Invalid object name 'dbo.UnknownT'able'.", exception.Message);
    }

    [TestMethod]
    public async Task SQLInjectionAttempt_Query_ReturnError()
    {
        SqlException exception = await Assert.ThrowsExceptionAsync<SqlException>(async () => await TestRunner.ExecuteTableAsync("dbo.Users' -- do something else"));

        Assert.AreEqual("Invalid object name 'dbo.Users' -- do something else'.", exception.Message);
    }

    [TestMethod]
    public async Task TooLongName_Query_ReturnError()
    {
        InvalidOperationException exception = await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () => await TestRunner.ExecuteTableAsync($"dbo.{new string('x', 129)}"));

        Assert.AreEqual("The name of the table or schema cannot be more than 128 characters", exception.Message);
    }

    [TestMethod]
    public async Task TooLongSchema_Query_ReturnError()
    {
        InvalidOperationException exception = await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () => await TestRunner.ExecuteTableAsync($"{new string('x', 129)}.Users"));

        Assert.AreEqual("The name of the table or schema cannot be more than 128 characters", exception.Message);
    }

    [TestMethod]
    public async Task MaxLengthButIncorrectName_Query_ReturnError()
    {
        const string _schema = "xUIBEQ70IZ4M49JJPRC240BXRG460QSPXA8LNQGZVCKG0S8EMWFEG9L22THP7EOB3N5RWXWT9YFRYGA832FZ5SQ76271KVPSQ1YPT0SD052R6W30GFE7CX6IQYQDISG1";
        const string _tableName = "xXC15NX9HYBS0J8RHT6YHB9JJIPS2TWZQ2MA9C05I70WYG83LG877Q03X1XBGKLI5PRPQ2QZ74CFVMTQUOODFE2MARXI4069OUJTJB1VSAVO4XL32WHFH8HBKFNSQQHQ";

        SqlException exception = await Assert.ThrowsExceptionAsync<SqlException>(async () => await TestRunner.ExecuteTableAsync($"{_schema}.{_tableName}"));

        Assert.AreEqual("Invalid object name 'xUIBEQ70IZ4M49JJPRC240BXRG460QSPXA8LNQGZVCKG0S8EMWFEG9L22THP7EOB3N5RWXWT9YFRYGA832FZ5SQ76271KVPSQ1YPT0SD052R6W30GFE7CX6IQYQDISG1.xXC15NX9HYBS0J8RHT6YHB9JJIPS2TWZQ2MA9C05I70WYG83LG877Q03X1XBGKLI5PRPQ2QZ74CFVMTQUOODFE2MARXI4069OUJTJB1VSAVO4XL32WHFH8HBKFNSQQHQ'.", exception.Message);
    }

    #endregion
}

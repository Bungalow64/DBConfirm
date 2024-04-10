using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DBConfirm.Core.DataResults;
using System.Threading.Tasks;
using DBConfirm.Core.Data;
using DBConfirm.Core.Parameters;
using DBConfirm.Packages.SQLServer.MSTest;

namespace Sample.Core.MSTest.Tests.Tables;

[TestClass]
public class SpecialCharacterTableTests : MSTestBase
{
    private const string _schema = "dbo";
    private const string _tableName = "User's";

    #region Can Query
    [TestMethod]
    public async Task SpecialCharacterTable_CanQuery_NoBrackets_Success()
    {
        QueryResult results = await TestRunner.ExecuteTableAsync($"{_schema}.{_tableName}");

        results
            .AssertColumnsExist("Id", "Name");
    }

    [TestMethod]
    public async Task SpecialCharacterTable_CanQuery_SchemaBrackets_Success()
    {
        QueryResult results = await TestRunner.ExecuteTableAsync($"[{_schema}].{_tableName}");

        results
            .AssertColumnsExist("Id", "Name");
    }

    [TestMethod]
    public async Task SpecialCharacterTable_CanQuery_TableBrackets_Success()
    {
        QueryResult results = await TestRunner.ExecuteTableAsync($"{_schema}.[{_tableName}]");

        results
            .AssertColumnsExist("Id", "Name");
    }

    [TestMethod]
    public async Task SpecialCharacterTable_CanQuery_BothBrackets_Success()
    {
        QueryResult results = await TestRunner.ExecuteTableAsync($"[{_schema}].[{_tableName}]");

        results
            .AssertColumnsExist("Id", "Name");
    }

    [TestMethod]
    public async Task SpecialCharacterTable_CanQuery_WithSpaces_Success()
    {
        QueryResult results = await TestRunner.ExecuteTableAsync($"[{_schema}]   .   [{_tableName}]");

        results
            .AssertColumnsExist("Id", "Name");
    }

    [TestMethod]
    public async Task SpecialCharacterTable_CanQuery_UnequalBrackets_Success()
    {
        QueryResult results = await TestRunner.ExecuteTableAsync($"[{_schema}].{_tableName}]");

        results
            .AssertColumnsExist("Id", "Name");
    }

    #endregion

    #region Can Insert

    [TestMethod]
    public async Task SpecialCharacterTable_CanInsert_NoBrackets_Success()
    {
        await TestRunner.InsertDataAsync($"{_schema}.{_tableName}", new DataSetRow
        {
            ["Name"] = "Name1"
        });

        QueryResult results = await TestRunner.ExecuteTableAsync($"[{_schema}].[{_tableName}]");

        results
            .AssertRowCount(1)
            .AssertValue(0, "Name", "Name1");
    }

    [TestMethod]
    public async Task SpecialCharacterTable_CanInsert_SchemaBrackets_Success()
    {
        await TestRunner.InsertDataAsync($"[{_schema}].{_tableName}", new DataSetRow
        {
            ["Name"] = "Name1"
        });

        QueryResult results = await TestRunner.ExecuteTableAsync($"[{_schema}].[{_tableName}]");

        results
            .AssertRowCount(1)
            .AssertValue(0, "Name", "Name1");
    }

    [TestMethod]
    public async Task SpecialCharacterTable_CanInsert_TableBrackets_Success()
    {
        await TestRunner.InsertDataAsync($"{_schema}.[{_tableName}]", new DataSetRow
        {
            ["Name"] = "Name1"
        });

        QueryResult results = await TestRunner.ExecuteTableAsync($"[{_schema}].[{_tableName}]");

        results
            .AssertRowCount(1)
            .AssertValue(0, "Name", "Name1");
    }

    [TestMethod]
    public async Task SpecialCharacterTable_CanInsert_BothBrackets_Success()
    {
        await TestRunner.InsertDataAsync($"[{_schema}].[{_tableName}]", new DataSetRow
        {
            ["Name"] = "Name1"
        });

        QueryResult results = await TestRunner.ExecuteTableAsync($"[{_schema}].[{_tableName}]");

        results
            .AssertRowCount(1)
            .AssertValue(0, "Name", "Name1");
    }

    [TestMethod]
    public async Task SpecialCharacterTable_CanInsert_WithSpaces_Success()
    {
        await TestRunner.InsertDataAsync($"[{_schema}]   .   [{_tableName}]", new DataSetRow
        {
            ["Name"] = "Name1"
        });

        QueryResult results = await TestRunner.ExecuteTableAsync($"[{_schema}].[{_tableName}]");

        results
            .AssertRowCount(1)
            .AssertValue(0, "Name", "Name1");
    }

    #endregion
}

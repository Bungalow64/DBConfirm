﻿using DBConfirm.Core.Data;
using DBConfirm.Core.DataResults;
using DBConfirm.Core.TestFrameworks.Abstract;
using DBConfirm.Frameworks.MSTest;
using NUnit.Framework;
using System;
using System.Data;
using System.Linq;

namespace Core.Tests.DataResults;

[TestFixture]
public class QueryResultTests
{
    #region Setup

    private readonly ITestFramework _testFramework = new MSTestFramework();

    private static DataTable CreateDefaultTable()
    {
        DataTable table = new();
        table.Columns.Add("UserId", typeof(int));
        table.Columns.Add("DomainId", typeof(int));
        return table;
    }
    private static void AddRow(DataTable table, int userId, int domainId)
    {
        DataRow row = table.NewRow();
        row["UserId"] = userId;
        row["DomainId"] = domainId;
        table.Rows.Add(row);
    }

    #endregion

    #region Ctor

    [Test]
    public void QueryResult_Ctor_Default_EmptyTableSet()
    {
        QueryResult queryResult = new(_testFramework);

        Assert.AreEqual(0, queryResult.RawData.Rows.Count);
    }

    [Test]
    public void QueryResult_Ctor_Default_NullTableSet()
    {
        QueryResult queryResult = new(null);

        Assert.AreEqual(0, queryResult.RawData.Rows.Count);
    }

    [Test]
    public void QueryResult_Ctor_SetData()
    {
        DataTable table = CreateDefaultTable();

        AddRow(table, 1001, 1002);
        AddRow(table, 2001, 2002);
        AddRow(table, 3001, 3002);

        QueryResult queryResult = new(_testFramework, table);

        Assert.AreEqual(3, queryResult.RawData.Rows.Count);
    }

    #endregion

    #region TotalRows

    [Test]
    public void QueryResult_TotalRows_DefaultConstructor_0()
    {
        QueryResult queryResult = new(_testFramework);

        Assert.AreEqual(0, queryResult.TotalRows);
    }

    [Test]
    public void QueryResult_TotalRows_NullConstructor_0()
    {
        QueryResult queryResult = new(null);

        Assert.AreEqual(0, queryResult.TotalRows);
    }

    [TestCase(0, 0)]
    [TestCase(1, 1)]
    [TestCase(2, 2)]
    public void QueryResult_TotalRows_CorrectValues(int actualRows, int expectedRows)
    {
        DataTable table = CreateDefaultTable();

        for (int x = 0; x < actualRows; x++)
        {
            AddRow(table, x, x);
        }

        QueryResult queryResult = new(_testFramework, table);

        Assert.AreEqual(expectedRows, queryResult.TotalRows);
    }

    #endregion

    #region TotalColumns

    [Test]
    public void QueryResult_TotalColumns_DefaultConstructor_0()
    {
        QueryResult queryResult = new(_testFramework);

        Assert.AreEqual(0, queryResult.TotalColumns);
    }

    [Test]
    public void QueryResult_TotalColumns_NullConstructor_0()
    {
        QueryResult queryResult = new(null);

        Assert.AreEqual(0, queryResult.TotalColumns);
    }

    [Test]
    public void QueryResult_TotalColumns_TableWithNoColumns_0()
    {
        DataTable table = new();

        QueryResult queryResult = new(_testFramework, table);

        Assert.AreEqual(0, queryResult.TotalColumns);
    }

    [Test]
    public void QueryResult_TotalColumns_TableWith2Columns_2()
    {
        DataTable table = new();
        table.Columns.Add("UserId", typeof(int));
        table.Columns.Add("DomainId", typeof(int));

        QueryResult queryResult = new(_testFramework, table);

        Assert.AreEqual(2, queryResult.TotalColumns);
    }

    #endregion

    #region ColumnNames

    [Test]
    public void QueryResult_ColumnNames_DefaultConstructor_0Items()
    {
        QueryResult queryResult = new(_testFramework);

        Assert.IsNotNull(queryResult.ColumnNames);
        Assert.AreEqual(0, queryResult.ColumnNames.Count);
    }

    [Test]
    public void QueryResult_ColumnNames_NullConstructor_0Items()
    {
        QueryResult queryResult = new(null);

        Assert.IsNotNull(queryResult.ColumnNames);
        Assert.AreEqual(0, queryResult.ColumnNames.Count);
    }

    [Test]
    public void QueryResult_ColumnNames_TableWithNoColumns_0Items()
    {
        DataTable table = new();

        QueryResult queryResult = new(_testFramework, table);

        Assert.IsNotNull(queryResult.ColumnNames);
        Assert.AreEqual(0, queryResult.ColumnNames.Count);
    }

    [Test]
    public void QueryResult_ColumnNames_TableWith2Columns_2Items()
    {
        DataTable table = new();
        table.Columns.Add("UserId", typeof(int));
        table.Columns.Add("DomainId", typeof(int));

        QueryResult queryResult = new(_testFramework, table);

        Assert.IsNotNull(queryResult.ColumnNames);
        Assert.AreEqual(2, queryResult.ColumnNames.Count);
        Assert.AreEqual("UserId", queryResult.ColumnNames.ElementAt(0));
        Assert.AreEqual("DomainId", queryResult.ColumnNames.ElementAt(1));
    }

    #endregion

    #region AssertRowCount

    [TestCase(0, 0)]
    [TestCase(1, 1)]
    [TestCase(2, 2)]
    public void QueryResult_AssertRowCount_ExpectCorrect(int actualRows, int expectedRows)
    {
        DataTable table = CreateDefaultTable();

        for (int x = 0; x < actualRows; x++)
        {
            AddRow(table, x, x);
        }

        QueryResult queryResult = new(_testFramework, table);

        Assert.DoesNotThrow(() =>
            queryResult.AssertRowCount(expectedRows));
    }

    [TestCase(0, 1)]
    [TestCase(1, 0)]
    [TestCase(2, 3)]
    public void QueryResult_AssertRowCount_ExpectIncorrect_ThrowError(int actualRows, int expectedRows)
    {
        DataTable table = CreateDefaultTable();

        for (int x = 0; x < actualRows; x++)
        {
            AddRow(table, x, x);
        }

        QueryResult queryResult = new(_testFramework, table);

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
            queryResult.AssertRowCount(expectedRows));

        Assert.AreEqual($"Assert.AreEqual failed. Expected:<{expectedRows}>. Actual:<{actualRows}>. The total row count is unexpected", exception.Message);
    }

    #endregion

    #region AssertColumnCount

    [TestCase(0, 0)]
    [TestCase(1, 1)]
    [TestCase(2, 2)]
    public void QueryResult_AssertColumnCount_ExpectCorrect(int actualColumns, int expectedColumns)
    {
        DataTable table = new();
        for (int x = 0; x < actualColumns; x++)
        {
            table.Columns.Add($"Column{x}", typeof(int));
        }

        QueryResult queryResult = new(_testFramework, table);

        Assert.DoesNotThrow(() =>
            queryResult.AssertColumnCount(expectedColumns));
    }

    [TestCase(0, 1)]
    [TestCase(1, 0)]
    [TestCase(2, 3)]
    public void QueryResult_AssertColumnCount_ExpectIncorrect_ThrowError(int actualColumns, int expectedColumns)
    {
        DataTable table = new();

        for (int x = 0; x < actualColumns; x++)
        {
            table.Columns.Add($"Column{x}", typeof(int));
        }

        QueryResult queryResult = new(_testFramework, table);

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
            queryResult.AssertColumnCount(expectedColumns));

        Assert.AreEqual($"Assert.AreEqual failed. Expected:<{expectedColumns}>. Actual:<{actualColumns}>. The total column count is unexpected", exception.Message);
    }

    #endregion

    #region AssertColumnExists

    [TestCase("UserId")]
    [TestCase("DomainId")]
    public void QueryResult_AssertColumnExists_ExpectCorrect(string columnName)
    {
        DataTable table = new();
        table.Columns.Add("UserId", typeof(int));
        table.Columns.Add("DomainId", typeof(int));

        QueryResult queryResult = new(_testFramework, table);

        Assert.DoesNotThrow(() =>
            queryResult.AssertColumnExists(columnName));
    }

    [TestCase("otherColumn")]
    [TestCase("userid")]
    [TestCase("USERID")]
    public void QueryResult_AssertColumnExists_ExpectIncorrect(string columnName)
    {
        DataTable table = new();
        table.Columns.Add("UserId", typeof(int));
        table.Columns.Add("DomainId", typeof(int));

        QueryResult queryResult = new(_testFramework, table);

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
            queryResult.AssertColumnExists(columnName));

        Assert.AreEqual($"CollectionAssert.Contains failed. Expected column {columnName} to be found but the only columns found are UserId, DomainId", exception.Message);
    }

    [Test]
    public void QueryResult_AssertColumnExists_OnlyOneColumnFound_ErrorMessageCorrect()
    {
        DataTable table = new();
        table.Columns.Add("UserId", typeof(int));

        QueryResult queryResult = new(_testFramework, table);

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
            queryResult.AssertColumnExists("DomainId"));

        Assert.AreEqual($"CollectionAssert.Contains failed. Expected column DomainId to be found but the only column found is UserId", exception.Message);
    }

    [Test]
    public void QueryResult_AssertColumnExists_NullColumn_ExistingColumns_Error()
    {
        DataTable table = new();
        table.Columns.Add("UserId", typeof(int));
        table.Columns.Add("DomainId", typeof(int));

        QueryResult queryResult = new(_testFramework, table);

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
            queryResult.AssertColumnExists(null));

        Assert.AreEqual($"CollectionAssert.Contains failed. Expected column <null> to be found but the only columns found are UserId, DomainId", exception.Message);
    }

    [Test]
    public void QueryResult_AssertColumnExists_NullColumn_NoColumns_Error()
    {
        DataTable table = new();

        QueryResult queryResult = new(_testFramework, table);

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
            queryResult.AssertColumnExists(null));

        Assert.AreEqual($"CollectionAssert.Contains failed. Expected column <null> to be found but no columns were found", exception.Message);
    }

    [Test]
    public void QueryResult_AssertColumnExists_ExpectedColumn_NoColumns_Error()
    {
        DataTable table = new();

        QueryResult queryResult = new(_testFramework, table);

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
            queryResult.AssertColumnExists("UserId"));

        Assert.AreEqual($"CollectionAssert.Contains failed. Expected column UserId to be found but no columns were found", exception.Message);
    }

    #endregion

    #region AssertColumnNotExists

    [TestCase("otherColumn")]
    [TestCase("userid")]
    [TestCase("USERID")]
    public void QueryResult_AssertColumnNotExists_ExpectCorrect(string columnName)
    {
        DataTable table = new();
        table.Columns.Add("UserId", typeof(int));
        table.Columns.Add("DomainId", typeof(int));

        QueryResult queryResult = new(_testFramework, table);

        Assert.DoesNotThrow(() =>
            queryResult.AssertColumnNotExists(columnName));
    }

    [TestCase("UserId")]
    [TestCase("DomainId")]
    public void QueryResult_AssertColumnNotExist_ExpectIncorrect(string columnName)
    {
        DataTable table = new();
        table.Columns.Add("UserId", typeof(int));
        table.Columns.Add("DomainId", typeof(int));

        QueryResult queryResult = new(_testFramework, table);

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
            queryResult.AssertColumnNotExists(columnName));

        Assert.AreEqual($"CollectionAssert.DoesNotContain failed. Expected column {columnName} to not be found but it was found", exception.Message);
    }

    [Test]
    public void QueryResult_AssertColumnNotExists_NullColumn_ExistingColumns_NoError()
    {
        DataTable table = new();
        table.Columns.Add("UserId", typeof(int));
        table.Columns.Add("DomainId", typeof(int));

        QueryResult queryResult = new(_testFramework, table);

        Assert.DoesNotThrow(() =>
            queryResult.AssertColumnNotExists(null));
    }

    [Test]
    public void QueryResult_AssertColumnNotExists_NullColumn_NoColumns_NoError()
    {
        DataTable table = new();

        QueryResult queryResult = new(_testFramework, table);

        Assert.DoesNotThrow(() =>
            queryResult.AssertColumnNotExists(null));
    }

    [Test]
    public void QueryResult_AssertColumnNotExists_ExpectedColumn_NoColumns_NoError()
    {
        DataTable table = new();

        QueryResult queryResult = new(_testFramework, table);

        Assert.DoesNotThrow(() =>
            queryResult.AssertColumnNotExists("UserId"));
    }

    #endregion

    #region AssertColumnsExist

    [TestCase("UserId")]
    [TestCase("DomainId")]
    public void QueryResult_AssertColumnsExist_SingleColumn_ExpectCorrect(string columnName)
    {
        DataTable table = new();
        table.Columns.Add("UserId", typeof(int));
        table.Columns.Add("DomainId", typeof(int));

        QueryResult queryResult = new(_testFramework, table);

        Assert.DoesNotThrow(() =>
            queryResult.AssertColumnsExist(columnName));
    }

    [TestCase("UserId", "DomainId")]
    [TestCase("DomainId", "UserId")]
    [TestCase("AddressId1", "UserId")]
    public void QueryResult_AssertColumnsExist_TwoColumns_ExpectCorrect(string columnName1, string columnName2)
    {
        DataTable table = new();
        table.Columns.Add("UserId", typeof(int));
        table.Columns.Add("DomainId", typeof(int));
        table.Columns.Add("AddressId1", typeof(int));

        QueryResult queryResult = new(_testFramework, table);

        Assert.DoesNotThrow(() =>
            queryResult.AssertColumnsExist(columnName1, columnName2));
    }

    [TestCase("otherColumn")]
    [TestCase("userid")]
    [TestCase("USERID")]
    public void QueryResult_AssertColumnsExist_ExpectIncorrect(string columnName)
    {
        DataTable table = new();
        table.Columns.Add("UserId", typeof(int));
        table.Columns.Add("DomainId", typeof(int));

        QueryResult queryResult = new(_testFramework, table);

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
            queryResult.AssertColumnsExist(columnName));

        Assert.AreEqual($"CollectionAssert.Contains failed. Expected column {columnName} to be found but the only columns found are UserId, DomainId", exception.Message);
    }

    [TestCase("otherColumn", "UserId")]
    [TestCase("userid", "UserId")]
    [TestCase("USERID", "UserId")]
    [TestCase("otherColumn", "DomainId")]
    public void QueryResult_AssertColumnsExist_FirstColumnMissing_ExpectIncorrect(string columnName1, string columnName2)
    {
        DataTable table = new();
        table.Columns.Add("UserId", typeof(int));
        table.Columns.Add("DomainId", typeof(int));

        QueryResult queryResult = new(_testFramework, table);

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
            queryResult.AssertColumnsExist(columnName1, columnName2));

        Assert.AreEqual($"CollectionAssert.Contains failed. Expected column {columnName1} to be found but the only columns found are UserId, DomainId", exception.Message);
    }

    [TestCase("UserId", "otherColumn")]
    [TestCase("DomainId", "userid")]
    public void QueryResult_AssertColumnsExist_SecondColumnMissing_ExpectIncorrect(string columnName1, string columnName2)
    {
        DataTable table = new();
        table.Columns.Add("UserId", typeof(int));
        table.Columns.Add("DomainId", typeof(int));

        QueryResult queryResult = new(_testFramework, table);

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
            queryResult.AssertColumnsExist(columnName1, columnName2));

        Assert.AreEqual($"CollectionAssert.Contains failed. Expected column {columnName2} to be found but the only columns found are UserId, DomainId", exception.Message);
    }

    [TestCase("otherColumn", "userid")]
    [TestCase("userid", "otherColumn")]
    public void QueryResult_AssertColumnsExist_BothColumnsMissing_ExpectIncorrect_ShowFirst(string columnName1, string columnName2)
    {
        DataTable table = new();
        table.Columns.Add("UserId", typeof(int));
        table.Columns.Add("DomainId", typeof(int));

        QueryResult queryResult = new(_testFramework, table);

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
            queryResult.AssertColumnsExist(columnName1, columnName2));

        Assert.AreEqual($"CollectionAssert.Contains failed. Expected column {columnName1} to be found but the only columns found are UserId, DomainId", exception.Message);
    }

    [Test]
    public void QueryResult_AssertColumnsExist_NullColumn_ExistingColumns_Error()
    {
        DataTable table = new();
        table.Columns.Add("UserId", typeof(int));
        table.Columns.Add("DomainId", typeof(int));

        QueryResult queryResult = new(_testFramework, table);

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
            queryResult.AssertColumnsExist(null));

        Assert.AreEqual($"CollectionAssert.Contains failed. Expected column <null> to be found but the only columns found are UserId, DomainId", exception.Message);
    }

    [Test]
    public void QueryResult_AssertColumnsExist_TwoNullColumns_ExistingColumns_Error()
    {
        DataTable table = new();
        table.Columns.Add("UserId", typeof(int));
        table.Columns.Add("DomainId", typeof(int));

        QueryResult queryResult = new(_testFramework, table);

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
            queryResult.AssertColumnsExist(null, null));

        Assert.AreEqual($"CollectionAssert.Contains failed. Expected column <null> to be found but the only columns found are UserId, DomainId", exception.Message);
    }

    [Test]
    public void QueryResult_AssertColumnsExist_NullColumn_NoColumns_Error()
    {
        DataTable table = new();

        QueryResult queryResult = new(_testFramework, table);

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
            queryResult.AssertColumnsExist(null));

        Assert.AreEqual($"CollectionAssert.Contains failed. Expected column <null> to be found but no columns were found", exception.Message);
    }

    [Test]
    public void QueryResult_AssertColumnsExist_TwoNullColumns_NoColumns_Error()
    {
        DataTable table = new();

        QueryResult queryResult = new(_testFramework, table);

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
            queryResult.AssertColumnsExist(null, null));

        Assert.AreEqual($"CollectionAssert.Contains failed. Expected column <null> to be found but no columns were found", exception.Message);
    }

    [Test]
    public void QueryResult_AssertColumnsExist_ExpectedColumn_NoColumns_Error()
    {
        DataTable table = new();

        QueryResult queryResult = new(_testFramework, table);

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
            queryResult.AssertColumnsExist("UserId"));

        Assert.AreEqual($"CollectionAssert.Contains failed. Expected column UserId to be found but no columns were found", exception.Message);
    }

    [Test]
    public void QueryResult_AssertColumnsExist_TwoExpectedColumns_NoColumns_Error()
    {
        DataTable table = new();

        QueryResult queryResult = new(_testFramework, table);

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
            queryResult.AssertColumnsExist("UserId", "DomainId"));

        Assert.AreEqual($"CollectionAssert.Contains failed. Expected column UserId to be found but no columns were found", exception.Message);
    }

    #endregion

    #region AssertColumnsNotExist

    [TestCase("otherColumn")]
    [TestCase("userid")]
    [TestCase("USERID")]
    public void QueryResult_AssertColumnsNotExist_ExpectCorrect(string columnName)
    {
        DataTable table = new();
        table.Columns.Add("UserId", typeof(int));
        table.Columns.Add("DomainId", typeof(int));

        QueryResult queryResult = new(_testFramework, table);

        Assert.DoesNotThrow(() =>
            queryResult.AssertColumnsNotExist(columnName));
    }

    [TestCase("otherColumn", "otherColumn2")]
    [TestCase("userid", "otherColumn2")]
    [TestCase("USERID", "otherColumn2")]
    public void QueryResult_AssertColumnsNotExist_MultipleColumns_ExpectCorrect(string columnName1, string columnName2)
    {
        DataTable table = new();
        table.Columns.Add("UserId", typeof(int));
        table.Columns.Add("DomainId", typeof(int));

        QueryResult queryResult = new(_testFramework, table);

        Assert.DoesNotThrow(() =>
            queryResult.AssertColumnsNotExist(columnName1, columnName2));
    }

    [TestCase("UserId")]
    [TestCase("DomainId")]
    public void QueryResult_AssertColumnsNotExist_ExpectIncorrect(string columnName)
    {
        DataTable table = new();
        table.Columns.Add("UserId", typeof(int));
        table.Columns.Add("DomainId", typeof(int));

        QueryResult queryResult = new(_testFramework, table);

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
            queryResult.AssertColumnsNotExist(columnName));

        Assert.AreEqual($"CollectionAssert.DoesNotContain failed. Expected column {columnName} to not be found but it was found", exception.Message);
    }

    [TestCase("UserId", "otherColumn")]
    [TestCase("DomainId", "otherColumn")]
    public void QueryResult_AssertColumnsNotExist_MultipleColumnsFirstFound_ExpectIncorrect(string columnName1, string columnName2)
    {
        DataTable table = new();
        table.Columns.Add("UserId", typeof(int));
        table.Columns.Add("DomainId", typeof(int));

        QueryResult queryResult = new(_testFramework, table);

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
            queryResult.AssertColumnsNotExist(columnName1, columnName2));

        Assert.AreEqual($"CollectionAssert.DoesNotContain failed. Expected column {columnName1} to not be found but it was found", exception.Message);
    }

    [TestCase("otherColumn", "UserId")]
    [TestCase("otherColumn", "DomainId")]
    public void QueryResult_AssertColumnsNotExist_MultipleColumnsSecondFound_ExpectIncorrect(string columnName1, string columnName2)
    {
        DataTable table = new();
        table.Columns.Add("UserId", typeof(int));
        table.Columns.Add("DomainId", typeof(int));

        QueryResult queryResult = new(_testFramework, table);

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
            queryResult.AssertColumnsNotExist(columnName1, columnName2));

        Assert.AreEqual($"CollectionAssert.DoesNotContain failed. Expected column {columnName2} to not be found but it was found", exception.Message);
    }

    [Test]
    public void QueryResult_AssertColumnsNotExist_NullColumn_ExistingColumns_NoError()
    {
        DataTable table = new();
        table.Columns.Add("UserId", typeof(int));
        table.Columns.Add("DomainId", typeof(int));

        QueryResult queryResult = new(_testFramework, table);

        Assert.DoesNotThrow(() =>
            queryResult.AssertColumnsNotExist(null));
    }

    [Test]
    public void QueryResult_AssertColumnsNotExist_NullColumns_ExistingColumns_NoError()
    {
        DataTable table = new();
        table.Columns.Add("UserId", typeof(int));
        table.Columns.Add("DomainId", typeof(int));

        QueryResult queryResult = new(_testFramework, table);

        Assert.DoesNotThrow(() =>
            queryResult.AssertColumnsNotExist(null, null));
    }

    [Test]
    public void QueryResult_AssertColumnsNotExist_NullColumn_NoColumns_NoError()
    {
        DataTable table = new();

        QueryResult queryResult = new(_testFramework, table);

        Assert.DoesNotThrow(() =>
            queryResult.AssertColumnsNotExist(null));
    }

    [Test]
    public void QueryResult_AssertColumnsNotExist_ExpectedColumn_NoColumns_NoError()
    {
        DataTable table = new();

        QueryResult queryResult = new(_testFramework, table);

        Assert.DoesNotThrow(() =>
            queryResult.AssertColumnsNotExist("UserId"));
    }

    #endregion

    #region AssertColumnValuesUnique

    [Test]
    public void QueryResult_AssertColumnValuesUnique_EmptyColumnNames_Failure()
    {
        DataTable table = new();
        table.Columns.Add("UserId", typeof(int));
        table.Columns.Add("DomainId", typeof(int));

        QueryResult queryResult = new(_testFramework, table);

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
            queryResult.AssertColumnValuesUnique());

        Assert.AreEqual($"Assert.Fail failed. No column names provided.  Specify columns to check for uniqueness", exception.Message);
    }

    [Test]
    public void QueryResult_AssertColumnValuesUnique_NullColumnNames_Failure()
    {
        DataTable table = new();
        table.Columns.Add("UserId", typeof(int));
        table.Columns.Add("DomainId", typeof(int));

        QueryResult queryResult = new(_testFramework, table);

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
            queryResult.AssertColumnValuesUnique(null));

        Assert.AreEqual($"Assert.Fail failed. No column names provided.  Specify columns to check for uniqueness", exception.Message);
    }

    [TestCase("OtherId")]
    [TestCase("DomainId2")]
    [TestCase("domainId")]
    public void QueryResult_AssertColumnValuesUnique_SpecifyColumnNotAvailable_Failure(string columnName)
    {
        DataTable table = new();
        table.Columns.Add("UserId", typeof(int));
        table.Columns.Add("DomainId", typeof(int));

        QueryResult queryResult = new(_testFramework, table);

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
            queryResult.AssertColumnValuesUnique(columnName));

        Assert.AreEqual($"CollectionAssert.Contains failed. Expected column {columnName} to be found but the only columns found are UserId, DomainId", exception.Message);
    }

    [Test]
    public void QueryResult_AssertColumnValuesUnique_SpecifyColumnsNotAvailable_Failure()
    {
        DataTable table = new();
        table.Columns.Add("UserId", typeof(int));
        table.Columns.Add("DomainId", typeof(int));

        QueryResult queryResult = new(_testFramework, table);

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
            queryResult.AssertColumnValuesUnique("Other1", "Other2"));

        Assert.AreEqual($"CollectionAssert.Contains failed. Expected column Other1 to be found but the only columns found are UserId, DomainId", exception.Message);
    }

    [Test]
    public void QueryResult_AssertColumnValuesUnique_SpecifySomeColumnsNotAvailable_Failure()
    {
        DataTable table = new();
        table.Columns.Add("UserId", typeof(int));
        table.Columns.Add("DomainId", typeof(int));

        QueryResult queryResult = new(_testFramework, table);

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
            queryResult.AssertColumnValuesUnique("DomainId", "Other1", "Other2"));

        Assert.AreEqual($"CollectionAssert.Contains failed. Expected column Other1 to be found but the only columns found are UserId, DomainId", exception.Message);
    }

    [Test]
    public void QueryResult_AssertColumnValuesUnique_SpecifyDuplicateColumnsNotAvailable_Failure()
    {
        DataTable table = new();
        table.Columns.Add("UserId", typeof(int));
        table.Columns.Add("DomainId", typeof(int));

        QueryResult queryResult = new(_testFramework, table);

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
            queryResult.AssertColumnValuesUnique("Other1", "Other1"));

        Assert.AreEqual($"CollectionAssert.Contains failed. Expected column Other1 to be found but the only columns found are UserId, DomainId", exception.Message);
    }

    [TestCase("UserId")]
    [TestCase("DomainId")]
    public void QueryResult_AssertColumnValuesUnique_SingleColumn_DataUnique_NoFailure(string columnName)
    {
        DataTable table = CreateDefaultTable();

        AddRow(table, 1, 2);
        AddRow(table, 3, 4);
        AddRow(table, 5, 6);

        QueryResult queryResult = new(_testFramework, table);

        Assert.DoesNotThrow(() =>
            queryResult.AssertColumnValuesUnique(columnName));
    }

    [Test]
    public void QueryResult_AssertColumnValuesUnique_MultipleColumns_DataUnique_NoFailure()
    {
        DataTable table = CreateDefaultTable();

        AddRow(table, 1, 2);
        AddRow(table, 3, 4);
        AddRow(table, 5, 6);

        QueryResult queryResult = new(_testFramework, table);

        Assert.DoesNotThrow(() =>
            queryResult.AssertColumnValuesUnique("UserId", "DomainId"));
    }

    [Test]
    public void QueryResult_AssertColumnValuesUnique_MultipleColumns_DataUniqueInOneColumnOnly_NoFailure()
    {
        DataTable table = CreateDefaultTable();

        AddRow(table, 1, 2);
        AddRow(table, 3, 2);
        AddRow(table, 5, 2);

        QueryResult queryResult = new(_testFramework, table);

        Assert.DoesNotThrow(() =>
            queryResult.AssertColumnValuesUnique("UserId", "DomainId"));
    }

    [Test]
    public void QueryResult_AssertColumnValuesUnique_SingleColumn_DataUniqueInColumnOnly_NoFailure()
    {
        DataTable table = CreateDefaultTable();

        AddRow(table, 1, 2);
        AddRow(table, 3, 2);
        AddRow(table, 5, 2);

        QueryResult queryResult = new(_testFramework, table);

        Assert.DoesNotThrow(() =>
            queryResult.AssertColumnValuesUnique("UserId"));
    }

    [Test]
    public void QueryResult_AssertColumnValuesUnique_SingleColumn_DataUniqueInOtherColumnOnly_Failure()
    {
        DataTable table = CreateDefaultTable();

        AddRow(table, 1, 2);
        AddRow(table, 3, 7);
        AddRow(table, 5, 7);

        QueryResult queryResult = new(_testFramework, table);

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
            queryResult.AssertColumnValuesUnique("DomainId"));

        Assert.AreEqual($"Assert.Fail failed. Duplicate data found for column DomainId in rows 1, 2", exception.Message);
    }

    [Test]
    public void QueryResult_AssertColumnValuesUnique_MultipleColumns_SomeDataNotUnique_Failure()
    {
        DataTable table = CreateDefaultTable();

        AddRow(table, 1, 2);
        AddRow(table, 3, 2);
        AddRow(table, 3, 2);

        QueryResult queryResult = new(_testFramework, table);

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
            queryResult.AssertColumnValuesUnique("UserId", "DomainId"));

        Assert.AreEqual($"Assert.Fail failed. Duplicate data found for columns UserId, DomainId in rows 1, 2", exception.Message);
    }

    [Test]
    public void QueryResult_AssertColumnValuesUnique_MultipleColumns_AllDataNotUnique_Failure()
    {
        DataTable table = CreateDefaultTable();

        AddRow(table, 3, 2);
        AddRow(table, 3, 2);
        AddRow(table, 3, 2);

        QueryResult queryResult = new(_testFramework, table);

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
            queryResult.AssertColumnValuesUnique("UserId", "DomainId"));

        Assert.AreEqual($"Assert.Fail failed. Duplicate data found for columns UserId, DomainId in rows 0, 1, 2", exception.Message);
    }

    [TestCase("Col1")]
    [TestCase("Col2")]
    public void QueryResult_AssertColumnValuesUnique_Dates_SingleColumn_Unique_NoFailure(string columnName)
    {
        DataTable table = new();
        table.Columns.Add("Col1", typeof(DateTime));
        table.Columns.Add("Col2", typeof(DateTime));

        void AddRow(object o1, object o2)
        {
            DataRow row = table.NewRow();
            row["Col1"] = o1;
            row["Col2"] = o2;
            table.Rows.Add(row);
        }

        AddRow(DateTime.Parse("01-Mar-2023 09:23:32"), DateTime.Parse("02-Mar-2023 09:23:32"));
        AddRow(DateTime.Parse("03-Mar-2023 09:23:32"), DateTime.Parse("04-Mar-2023 09:23:32"));
        AddRow(DateTime.Parse("05-Mar-2023 09:23:32"), DateTime.Parse("06-Mar-2023 09:23:32"));
        AddRow(DateTime.Parse("06-Mar-2023 09:23:32"), DateTime.Parse("07-Mar-2023 09:23:32"));

        QueryResult queryResult = new(_testFramework, table);

        Assert.DoesNotThrow(() =>
            queryResult.AssertColumnValuesUnique(columnName));
    }

    [Test]
    public void QueryResult_AssertColumnValuesUnique_Dates_MultipleColumns_Unique_NoFailure()
    {
        DataTable table = new();
        table.Columns.Add("Col1", typeof(DateTime));
        table.Columns.Add("Col2", typeof(DateTime));

        void AddRow(object o1, object o2)
        {
            DataRow row = table.NewRow();
            row["Col1"] = o1;
            row["Col2"] = o2;
            table.Rows.Add(row);
        }

        AddRow(DateTime.Parse("01-Mar-2023 09:23:32"), DateTime.Parse("02-Mar-2023 09:23:32"));
        AddRow(DateTime.Parse("03-Mar-2023 09:23:32"), DateTime.Parse("04-Mar-2023 09:23:32"));
        AddRow(DateTime.Parse("03-Mar-2023 09:23:32"), DateTime.Parse("06-Mar-2023 09:23:32"));
        AddRow(DateTime.Parse("06-Mar-2023 09:23:32"), DateTime.Parse("02-Mar-2023 09:23:32"));

        QueryResult queryResult = new(_testFramework, table);

        Assert.DoesNotThrow(() =>
            queryResult.AssertColumnValuesUnique("Col1", "Col2"));
    }

    [Test]
    public void QueryResult_AssertColumnValuesUnique_Dates_NotUnique_Failure()
    {
        DataTable table = new();
        table.Columns.Add("Col1", typeof(DateTime));
        table.Columns.Add("Col2", typeof(DateTime));

        void AddRow(object o1, object o2)
        {
            DataRow row = table.NewRow();
            row["Col1"] = o1;
            row["Col2"] = o2;
            table.Rows.Add(row);
        }

        AddRow(DateTime.Parse("01-Mar-2023 09:23:32"), DateTime.Parse("02-Mar-2023 09:23:32"));
        AddRow(DateTime.Parse("03-Mar-2023 09:23:32"), DateTime.Parse("04-Mar-2023 09:23:32"));
        AddRow(DateTime.Parse("06-Mar-2023 09:23:32"), DateTime.Parse("07-Mar-2023 09:23:32"));
        AddRow(DateTime.Parse("03-Mar-2023 09:23:32"), DateTime.Parse("04-Mar-2023 09:23:32"));

        QueryResult queryResult = new(_testFramework, table);

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
            queryResult.AssertColumnValuesUnique("Col1", "Col2"));

        Assert.AreEqual($"Assert.Fail failed. Duplicate data found for columns Col1, Col2 in rows 1, 3", exception.Message);
    }

    [TestCase("Col1")]
    [TestCase("Col2")]
    public void QueryResult_AssertColumnValuesUnique_Strings_SingleColumn_Unique_NoFailure(string columnName)
    {
        DataTable table = new();
        table.Columns.Add("Col1", typeof(string));
        table.Columns.Add("Col2", typeof(string));

        void AddRow(object o1, object o2)
        {
            DataRow row = table.NewRow();
            row["Col1"] = o1;
            row["Col2"] = o2;
            table.Rows.Add(row);
        }

        AddRow("a", "b");
        AddRow("b", "c");
        AddRow("c", "d");
        AddRow("d", "e");

        QueryResult queryResult = new(_testFramework, table);

        Assert.DoesNotThrow(() =>
            queryResult.AssertColumnValuesUnique(columnName));
    }

    [Test]
    public void QueryResult_AssertColumnValuesUnique_Strings_NotUnique_Failure()
    {
        DataTable table = new();
        table.Columns.Add("Col1", typeof(string));
        table.Columns.Add("Col2", typeof(string));

        void AddRow(object o1, object o2)
        {
            DataRow row = table.NewRow();
            row["Col1"] = o1;
            row["Col2"] = o2;
            table.Rows.Add(row);
        }

        AddRow("a", "b");
        AddRow("b", "c");
        AddRow("c", "d");
        AddRow("d", "b");

        QueryResult queryResult = new(_testFramework, table);

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
            queryResult.AssertColumnValuesUnique("Col2"));

        Assert.AreEqual($"Assert.Fail failed. Duplicate data found for column Col2 in rows 0, 3", exception.Message);
    }

    [TestCase("Col1")]
    [TestCase("Col2")]
    public void QueryResult_AssertColumnValuesUnique_Bools_SingleColumn_Unique_NoFailure(string columnName)
    {
        DataTable table = new();
        table.Columns.Add("Col1", typeof(bool));
        table.Columns.Add("Col2", typeof(bool));

        void AddRow(object o1, object o2)
        {
            DataRow row = table.NewRow();
            row["Col1"] = o1;
            row["Col2"] = o2;
            table.Rows.Add(row);
        }

        AddRow(false, false);
        AddRow(true, true);

        QueryResult queryResult = new(_testFramework, table);

        Assert.DoesNotThrow(() =>
            queryResult.AssertColumnValuesUnique(columnName));
    }

    [Test]
    public void QueryResult_AssertColumnValuesUnique_Bools_MultipleColumns_Unique_NoFailure()
    {
        DataTable table = new();
        table.Columns.Add("Col1", typeof(bool));
        table.Columns.Add("Col2", typeof(bool));

        void AddRow(object o1, object o2)
        {
            DataRow row = table.NewRow();
            row["Col1"] = o1;
            row["Col2"] = o2;
            table.Rows.Add(row);
        }

        AddRow(false, false);
        AddRow(true, true);
        AddRow(false, true);
        AddRow(true, false);

        QueryResult queryResult = new(_testFramework, table);

        Assert.DoesNotThrow(() =>
            queryResult.AssertColumnValuesUnique("Col1", "Col2"));
    }

    [Test]
    public void QueryResult_AssertColumnValuesUnique_Bools_NotUnique_Failure()
    {
        DataTable table = new();
        table.Columns.Add("Col1", typeof(bool));
        table.Columns.Add("Col2", typeof(bool));

        void AddRow(object o1, object o2)
        {
            DataRow row = table.NewRow();
            row["Col1"] = o1;
            row["Col2"] = o2;
            table.Rows.Add(row);
        }

        AddRow(false, false);
        AddRow(true, false);

        QueryResult queryResult = new(_testFramework, table);

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
            queryResult.AssertColumnValuesUnique("Col2"));

        Assert.AreEqual($"Assert.Fail failed. Duplicate data found for column Col2 in rows 0, 1", exception.Message);
    }

    #endregion

    #region AssertRowPositionExists

    [TestCase(1, 0)]
    [TestCase(2, 0)]
    [TestCase(2, 1)]
    [TestCase(3, 0)]
    [TestCase(3, 1)]
    [TestCase(3, 2)]
    public void QueryResult_AssertRowPositionExists_ExpectCorrect(int totalRows, int expectedPosition)
    {
        DataTable table = CreateDefaultTable();

        for (int x = 0; x < totalRows; x++)
        {
            AddRow(table, x, x);
        }

        QueryResult queryResult = new(_testFramework, table);

        Assert.DoesNotThrow(() =>
            queryResult.AssertRowPositionExists(expectedPosition));
    }

    [TestCase(1, 1)]
    [TestCase(1, 2)]
    [TestCase(1, 3)]
    [TestCase(1, -1)]
    public void QueryResult_AssertRowPositionExists_1Row_ExpectIncorrect_ThrowError(int actualRows, int expectedRows)
    {
        DataTable table = CreateDefaultTable();

        for (int x = 0; x < actualRows; x++)
        {
            AddRow(table, x, x);
        }

        QueryResult queryResult = new(_testFramework, table);

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
            queryResult.AssertRowPositionExists(expectedRows));

        Assert.AreEqual($"Assert.IsTrue failed. There is no row at position {expectedRows} (zero-based).  There is 1 row", exception.Message);
    }

    [TestCase(2, 2)]
    [TestCase(2, 3)]
    public void QueryResult_AssertRowPositionExists_2Rows_ExpectIncorrect_ThrowError(int actualRows, int expectedRows)
    {
        DataTable table = CreateDefaultTable();

        for (int x = 0; x < actualRows; x++)
        {
            AddRow(table, x, x);
        }

        QueryResult queryResult = new(_testFramework, table);

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
            queryResult.AssertRowPositionExists(expectedRows));

        Assert.AreEqual($"Assert.IsTrue failed. There is no row at position {expectedRows} (zero-based).  There are 2 rows", exception.Message);
    }

    [TestCase(0, 0)]
    [TestCase(0, 1)]
    [TestCase(0, 2)]
    [TestCase(0, -1)]
    public void QueryResult_AssertRowPositionExists_0Rows_ExpectIncorrect_ThrowError(int actualRows, int expectedRows)
    {
        DataTable table = CreateDefaultTable();

        for (int x = 0; x < actualRows; x++)
        {
            AddRow(table, x, x);
        }

        QueryResult queryResult = new(_testFramework, table);

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
            queryResult.AssertRowPositionExists(expectedRows));

        Assert.AreEqual($"Assert.IsTrue failed. There is no row at position {expectedRows} (zero-based).  There are 0 rows", exception.Message);
    }

    #endregion

    #region AssertValue

    [TestCase(0, "UserId", 1001)]
    [TestCase(0, "DomainId", 1002)]
    [TestCase(1, "UserId", 2001)]
    [TestCase(1, "DomainId", 2002)]
    [TestCase(2, "UserId", 3001)]
    [TestCase(2, "DomainId", 3002)]
    public void QueryResult_AssertValue_ValueIsCorrect_NoError(int rowNumber, string columnName, int expectedValue)
    {
        DataTable table = CreateDefaultTable();
        AddRow(table, 1001, 1002);
        AddRow(table, 2001, 2002);
        AddRow(table, 3001, 3002);

        QueryResult queryResult = new(_testFramework, table);

        Assert.DoesNotThrow(() =>
            queryResult.AssertValue(rowNumber, columnName, expectedValue));
    }

    [TestCase(-1, 1001)]
    [TestCase(3, 1001)]
    public void QueryResult_AssertValue_RowDoesNotExist_Error(int rowNumber, int expectedUserId)
    {
        DataTable table = CreateDefaultTable();
        AddRow(table, 1001, 1002);
        AddRow(table, 2001, 2002);
        AddRow(table, 3001, 3002);

        QueryResult queryResult = new(_testFramework, table);

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
            queryResult.AssertValue(rowNumber, "UserId", expectedUserId));

        Assert.AreEqual($"Assert.IsTrue failed. There is no row at position {rowNumber} (zero-based).  There are 3 rows", exception.Message);
    }

    [TestCase("OtherColumn", 1001)]
    [TestCase("userid", 1001)]
    [TestCase("USERID", 1001)]
    public void QueryResult_AssertValue_ColumnDoesNotExist_Error(string columnName, int expectedValue)
    {
        DataTable table = CreateDefaultTable();
        AddRow(table, 1001, 1002);

        QueryResult queryResult = new(_testFramework, table);

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
            queryResult.AssertValue(0, columnName, expectedValue));

        Assert.AreEqual($"CollectionAssert.Contains failed. Expected column {columnName} to be found but the only columns found are UserId, DomainId", exception.Message);
    }

    [TestCase(0, "UserId", 1502)]
    [TestCase(0, "DomainId", 1502)]
    [TestCase(1, "UserId", 2502)]
    [TestCase(1, "DomainId", 2502)]
    [TestCase(2, "UserId", 3502)]
    [TestCase(2, "DomainId", 3502)]
    public void QueryResult_AssertValue_ValueDoesNotMatch_Error(int rowNumber, string columnName, int expectedUserId)
    {
        DataTable table = CreateDefaultTable();
        AddRow(table, 1001, 1002);
        AddRow(table, 2001, 2002);
        AddRow(table, 3001, 3002);

        QueryResult queryResult = new(_testFramework, table);

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
            queryResult.AssertValue(rowNumber, columnName, expectedUserId));

        Assert.AreEqual($"Assert.AreEqual failed. Expected:<{expectedUserId}>. Actual:<{table.Rows[rowNumber][columnName]}>. Column {columnName} in row {rowNumber} has an unexpected value", exception.Message);
    }

    #endregion

    #region ValidateRow

    [TestCase(0)]
    [TestCase(1)]
    [TestCase(2)]
    public void QueryResult_ValidateRow_ValidRow(int rowNumber)
    {
        DataTable table = CreateDefaultTable();
        AddRow(table, 1001, 1002);
        AddRow(table, 2001, 2002);
        AddRow(table, 3001, 3002);

        QueryResult queryResult = new(_testFramework, table);

        RowResult rowResult = queryResult.ValidateRow(rowNumber);

        rowResult.AssertValue("UserId", table.Rows[rowNumber]["UserId"]);
    }

    [TestCase(-1)]
    [TestCase(3)]
    public void QueryResult_ValidateRow_RowDoesNotExist_Error(int rowNumber)
    {
        DataTable table = CreateDefaultTable();
        AddRow(table, 1001, 1002);
        AddRow(table, 2001, 2002);
        AddRow(table, 3001, 3002);

        QueryResult queryResult = new(_testFramework, table);

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
            { RowResult result = queryResult.ValidateRow(rowNumber); });

        Assert.AreEqual($"Assert.IsTrue failed. There is no row at position {rowNumber} (zero-based).  There are 3 rows", exception.Message);
    }

    #endregion

    #region AssertRowValues

    [TestCase(0, 1001, 1002)]
    [TestCase(1, 2001, 2002)]
    [TestCase(2, 3001, 3002)]
    public void QueryResult_AssertRowValues_ValidRow(int rowNumber, int expectedUserId, int expectedDomainId)
    {
        DataTable table = CreateDefaultTable();
        AddRow(table, 1001, 1002);
        AddRow(table, 2001, 2002);
        AddRow(table, 3001, 3002);

        QueryResult queryResult = new(_testFramework, table);

        DataSetRow expectedData = new()
        {
            { "UserId", expectedUserId },
            { "DomainId", expectedDomainId }
        };

        Assert.DoesNotThrow(() =>
            queryResult.AssertRowValues(rowNumber, expectedData));
    }

    [TestCase(-1)]
    [TestCase(3)]
    public void QueryResult_AssertRowValues_RowDoesNotExist_Error(int rowNumber)
    {
        DataTable table = CreateDefaultTable();
        AddRow(table, 1001, 1002);
        AddRow(table, 2001, 2002);
        AddRow(table, 3001, 3002);

        QueryResult queryResult = new(_testFramework, table);

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
        { queryResult.AssertRowValues(rowNumber, []); });

        Assert.AreEqual($"Assert.IsTrue failed. There is no row at position {rowNumber} (zero-based).  There are 3 rows", exception.Message);
    }

    [TestCase(0, 1501, 1002)]
    [TestCase(1, 2501, 2002)]
    [TestCase(2, 3501, 3002)]
    public void QueryResult_AssertRowValues_UserIdValuesDoNotMatch_Error(int rowNumber, int expectedUserId, int expectedDomainId)
    {
        DataTable table = CreateDefaultTable();
        AddRow(table, 1001, 1002);
        AddRow(table, 2001, 2002);
        AddRow(table, 3001, 3002);

        QueryResult queryResult = new(_testFramework, table);

        DataSetRow expectedData = new()
        {
            { "UserId", expectedUserId },
            { "DomainId", expectedDomainId }
        };

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
        { queryResult.AssertRowValues(rowNumber, expectedData); });

        Assert.AreEqual($"Assert.AreEqual failed. Expected:<{expectedUserId}>. Actual:<{table.Rows[rowNumber]["UserId"]}>. Column UserId in row {rowNumber} has an unexpected value", exception.Message);
    }

    [TestCase(0, 1001, 1502)]
    [TestCase(1, 2001, 2502)]
    [TestCase(2, 3001, 3502)]
    public void QueryResult_AssertRowValues_DomainIdValuesDoNotMatch_Error(int rowNumber, int expectedUserId, int expectedDomainId)
    {
        DataTable table = CreateDefaultTable();
        AddRow(table, 1001, 1002);
        AddRow(table, 2001, 2002);
        AddRow(table, 3001, 3002);

        QueryResult queryResult = new(_testFramework, table);

        DataSetRow expectedData = new()
        {
            { "UserId", expectedUserId },
            { "DomainId", expectedDomainId }
        };

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
        { queryResult.AssertRowValues(rowNumber, expectedData); });

        Assert.AreEqual($"Assert.AreEqual failed. Expected:<{expectedDomainId}>. Actual:<{table.Rows[rowNumber]["DomainId"]}>. Column DomainId in row {rowNumber} has an unexpected value", exception.Message);
    }

    #endregion

    #region AssertRowExists

    [TestCase(1001, 1002)]
    [TestCase(2001, 2002)]
    [TestCase(3001, 3002)]
    public void QueryResult_AssertRowExists_ValidRow(int expectedUserId, int expectedDomainId)
    {
        DataTable table = CreateDefaultTable();
        AddRow(table, 1001, 1002);
        AddRow(table, 2001, 2002);
        AddRow(table, 3001, 3002);

        QueryResult queryResult = new(_testFramework, table);

        DataSetRow expectedData = new()
        {
            { "UserId", expectedUserId },
            { "DomainId", expectedDomainId }
        };

        Assert.DoesNotThrow(() =>
            queryResult.AssertRowExists(expectedData));
    }

    [TestCase(1001, 1002)]
    [TestCase(2001, 2002)]
    [TestCase(3001, 3002)]
    public void QueryResult_AssertRowExists_DuplicateRows_ValidRow(int expectedUserId, int expectedDomainId)
    {
        DataTable table = CreateDefaultTable();
        AddRow(table, 1001, 1002);
        AddRow(table, 1001, 1002);
        AddRow(table, 2001, 2002);
        AddRow(table, 2001, 2002);
        AddRow(table, 3001, 3002);
        AddRow(table, 3001, 3002);

        QueryResult queryResult = new(_testFramework, table);

        DataSetRow expectedData = new()
        {
            { "UserId", expectedUserId },
            { "DomainId", expectedDomainId }
        };

        Assert.DoesNotThrow(() =>
            queryResult.AssertRowExists(expectedData));
    }

    [TestCase(1501, 1002)]
    [TestCase(2501, 2002)]
    [TestCase(3501, 3002)]
    [TestCase(1001, 1502)]
    [TestCase(2001, 2502)]
    [TestCase(3001, 3502)]
    [TestCase(1501, 1502)]
    [TestCase(2501, 2502)]
    [TestCase(3501, 3502)]
    public void QueryResult_AssertRowExists_ValuesDoNotMatch_Error(int expectedUserId, int expectedDomainId)
    {
        DataTable table = CreateDefaultTable();
        AddRow(table, 1001, 1002);
        AddRow(table, 2001, 2002);
        AddRow(table, 3001, 3002);

        QueryResult queryResult = new(_testFramework, table);

        DataSetRow expectedData = new()
        {
            { "UserId", expectedUserId },
            { "DomainId", expectedDomainId }
        };

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
        { queryResult.AssertRowExists(expectedData); });

        Assert.AreEqual(@$"Assert.Fail failed. No rows found matching the expected data: 
[UserId, {expectedUserId}]
[DomainId, {expectedDomainId}]", exception.Message);
    }

    [TestCase("otherDomainId")]
    [TestCase("userid")]
    [TestCase("USERID")]
    public void QueryResult_AssertRowExists_ColumnNameDoesNotExist_Error(string columnname)
    {
        DataTable table = CreateDefaultTable();
        AddRow(table, 1001, 1002);
        AddRow(table, 2001, 2002);
        AddRow(table, 3001, 3002);

        QueryResult queryResult = new(_testFramework, table);

        DataSetRow expectedData = new()
        {
            { columnname, 1000 }
        };

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
        { queryResult.AssertRowExists(expectedData); });

        Assert.AreEqual($"CollectionAssert.Contains failed. Expected column {columnname} to be found but the only columns found are UserId, DomainId", exception.Message);
    }

    #endregion

    #region AssertRowDoesNotExist

    [TestCase(1501, 1002)]
    [TestCase(2501, 2002)]
    [TestCase(3501, 3002)]
    [TestCase(1001, 1502)]
    [TestCase(2001, 2502)]
    [TestCase(3001, 3502)]
    [TestCase(1501, 1502)]
    [TestCase(2501, 2502)]
    [TestCase(3501, 3502)]
    public void QueryResult_AssertRowDoesNotExist_ValidRow(int expectedUserId, int expectedDomainId)
    {
        DataTable table = CreateDefaultTable();
        AddRow(table, 1001, 1002);
        AddRow(table, 2001, 2002);
        AddRow(table, 3001, 3002);

        QueryResult queryResult = new(_testFramework, table);

        DataSetRow expectedData = new()
        {
            { "UserId", expectedUserId },
            { "DomainId", expectedDomainId }
        };

        Assert.DoesNotThrow(() =>
            queryResult.AssertRowDoesNotExist(expectedData));
    }

    [TestCase(0)]
    [TestCase(1)]
    [TestCase(2)]
    public void QueryResult_AssertRowDoesNotExist_ValuesDoNotMatch_Error(int matchingRowNumber)
    {
        DataTable table = CreateDefaultTable();
        AddRow(table, 1001, 1002);
        AddRow(table, 2001, 2002);
        AddRow(table, 3001, 3002);

        QueryResult queryResult = new(_testFramework, table);

        DataSetRow expectedData = new()
        {
            { "UserId", table.Rows[matchingRowNumber]["UserId"] },
            { "DomainId", table.Rows[matchingRowNumber]["DomainId"] }
        };

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
        { queryResult.AssertRowDoesNotExist(expectedData); });

        Assert.AreEqual(@$"Assert.Fail failed. Row {matchingRowNumber} matches the expected data that should not match anything: 
[UserId, {expectedData["UserId"]}]
[DomainId, {expectedData["DomainId"]}]", exception.Message);
    }

    [TestCase("otherDomainId")]
    [TestCase("userid")]
    [TestCase("USERID")]
    public void QueryResult_AssertRowDoesNotExist_ColumnNameDoesNotExist_Error(string columnname)
    {
        DataTable table = CreateDefaultTable();
        AddRow(table, 1001, 1002);
        AddRow(table, 2001, 2002);
        AddRow(table, 3001, 3002);

        QueryResult queryResult = new(_testFramework, table);

        DataSetRow expectedData = new()
        {
            { columnname, 1000 }
        };

        var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
        { queryResult.AssertRowDoesNotExist(expectedData); });

        Assert.AreEqual($"CollectionAssert.Contains failed. Expected column {columnname} to be found but the only columns found are UserId, DomainId", exception.Message);
    }

    #endregion
}

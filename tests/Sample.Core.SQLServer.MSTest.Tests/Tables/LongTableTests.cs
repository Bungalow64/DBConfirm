using Microsoft.VisualStudio.TestTools.UnitTesting;
using DBConfirm.Core.DataResults;
using System.Threading.Tasks;
using DBConfirm.Core.Data;
using DBConfirm.Packages.SQLServer.MSTest;

namespace Sample.Core.SQLServer.MSTest.Tests.Tables
{
    [TestClass]
    public class LongTableTests : MSTestBase
    {
        private const string _schema = "NUIBEQ70IZ4M49JJPRC240BXRG460QSPXA8LNQGZVCKG0S8EMWFEG9L22THP7EOB3N5RWXWT9YFRYGA832FZ5SQ76271KVPSQ1YPT0SD052R6W30GFE7CX6IQYQDISG1";
        private const string _tableName = "FXC15NX9HYBS0J8RHT6YHB9JJIPS2TWZQ2MA9C05I70WYG83LG877Q03X1XBGKLI5PRPQ2QZ74CFVMTQUOODFE2MARXI4069OUJTJB1VSAVO4XL32WHFH8HBKFNSQQHQ";

        #region Can Query
        [TestMethod]
        public async Task LongTable_CanQuery_NoBrackets_Success()
        {
            QueryResult results = await TestRunner.ExecuteTableAsync($"{_schema}.{_tableName}");

            results
                .AssertColumnsExist("Id", "Name");
        }

        [TestMethod]
        public async Task LongTable_CanQuery_SchemaBrackets_Success()
        {
            QueryResult results = await TestRunner.ExecuteTableAsync($"[{_schema}].{_tableName}");

            results
                .AssertColumnsExist("Id", "Name");
        }

        [TestMethod]
        public async Task LongTable_CanQuery_TableBrackets_Success()
        {
            QueryResult results = await TestRunner.ExecuteTableAsync($"{_schema}.[{_tableName}]");

            results
                .AssertColumnsExist("Id", "Name");
        }

        [TestMethod]
        public async Task LongTable_CanQuery_BothBrackets_Success()
        {
            QueryResult results = await TestRunner.ExecuteTableAsync($"[{_schema}].[{_tableName}]");

            results
                .AssertColumnsExist("Id", "Name");
        }

        [TestMethod]
        public async Task LongTable_CanQuery_WithSpaces_Success()
        {
            QueryResult results = await TestRunner.ExecuteTableAsync($"[{_schema}]   .   [{_tableName}]");

            results
                .AssertColumnsExist("Id", "Name");
        }

        [TestMethod]
        public async Task LongTable_CanQuery_UnequalBrackets_Success()
        {
            QueryResult results = await TestRunner.ExecuteTableAsync($"[{_schema}].{_tableName}]");

            results
                .AssertColumnsExist("Id", "Name");
        }

        #endregion

        #region Can Insert

        [TestMethod]
        public async Task LongTable_CanInsert_NoBrackets_Success()
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
        public async Task LongTable_CanInsert_SchemaBrackets_Success()
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
        public async Task LongTable_CanInsert_TableBrackets_Success()
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
        public async Task LongTable_CanInsert_BothBrackets_Success()
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
        public async Task LongTable_CanInsert_WithSpaces_Success()
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
}

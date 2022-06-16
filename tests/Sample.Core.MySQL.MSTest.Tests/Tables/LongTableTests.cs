using Microsoft.VisualStudio.TestTools.UnitTesting;
using DBConfirm.Core.DataResults;
using System.Threading.Tasks;
using DBConfirm.Core.Data;
using DBConfirm.Packages.MySQL.MSTest;

namespace Sample.Core.MySQL.MSTest.Tests.Tables
{
    [TestClass]
    public class LongTableTests : MSTestBase
    {
        private const string _tableName = "FXC15NX9HYBS0J8RHT6YHB9JJIPS2TWZQ2MA9C05I70WYG83LG877Q03X1XBGKLI";

        #region Can Query
        [TestMethod]
        public async Task LongTable_CanQuery_NoBrackets_Success()
        {
            QueryResult results = await TestRunner.ExecuteTableAsync($"{_tableName}");

            results
                .AssertColumnsExist("Id", "Name");
        }

        [TestMethod]
        public async Task LongTable_CanQuery_TableBrackets_Success()
        {
            QueryResult results = await TestRunner.ExecuteTableAsync($"`{_tableName}`");

            results
                .AssertColumnsExist("Id", "Name");
        }

        [TestMethod]
        public async Task LongTable_CanQuery_WithSpaces_Success()
        {
            QueryResult results = await TestRunner.ExecuteTableAsync($"   `{_tableName}`");

            results
                .AssertColumnsExist("Id", "Name");
        }

        [TestMethod]
        public async Task LongTable_CanQuery_UnequalBrackets_Success()
        {
            QueryResult results = await TestRunner.ExecuteTableAsync($"{_tableName}`");

            results
                .AssertColumnsExist("Id", "Name");
        }

        #endregion

        #region Can Insert

        [TestMethod]
        public async Task LongTable_CanInsert_NoBrackets_Success()
        {
            await TestRunner.InsertDataAsync($"{_tableName}", new DataSetRow
            {
                ["Name"] = "Name1"
            });

            QueryResult results = await TestRunner.ExecuteTableAsync($"`{_tableName}`");

            results
                .AssertRowCount(1)
                .AssertValue(0, "Name", "Name1");
        }

        [TestMethod]
        public async Task LongTable_CanInsert_WithBrackets_Success()
        {
            await TestRunner.InsertDataAsync($"`{_tableName}`", new DataSetRow
            {
                ["Name"] = "Name1"
            });

            QueryResult results = await TestRunner.ExecuteTableAsync($"`{_tableName}`");

            results
                .AssertRowCount(1)
                .AssertValue(0, "Name", "Name1");
        }

        [TestMethod]
        public async Task LongTable_CanInsert_WithSpaces_Success()
        {
            await TestRunner.InsertDataAsync($"   `{_tableName}`", new DataSetRow
            {
                ["Name"] = "Name1"
            });

            QueryResult results = await TestRunner.ExecuteTableAsync($"`{_tableName}`");

            results
                .AssertRowCount(1)
                .AssertValue(0, "Name", "Name1");
        }

        [TestMethod]
        public async Task LongTable_CanInsert_UnequalBrackets_Success()
        {
            await TestRunner.InsertDataAsync($"{_tableName}`", new DataSetRow
            {
                ["Name"] = "Name1"
            });

            QueryResult results = await TestRunner.ExecuteTableAsync($"`{_tableName}`");

            results
                .AssertRowCount(1)
                .AssertValue(0, "Name", "Name1");
        }

        #endregion
    }
}

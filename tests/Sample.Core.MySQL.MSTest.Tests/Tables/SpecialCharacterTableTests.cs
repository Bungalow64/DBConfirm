using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DBConfirm.Core.DataResults;
using System.Threading.Tasks;
using DBConfirm.Core.Data;
using DBConfirm.Core.Parameters;
using DBConfirm.Packages.MySQL.MSTest;

namespace Sample.Core.MySQL.MSTest.Tests.Tables
{
    [TestClass]
    public class SpecialCharacterTableTests : MSTestBase
    {
        private const string _tableName = "User's";

        #region Can Query
        [TestMethod]
        public async Task SpecialCharacterTable_CanQuery_NoBrackets_Success()
        {
            QueryResult results = await TestRunner.ExecuteTableAsync($"{_tableName}");

            results
                .AssertColumnsExist("Id", "Name");
        }

        [TestMethod]
        public async Task SpecialCharacterTable_CanQuery_TableBrackets_Success()
        {
            QueryResult results = await TestRunner.ExecuteTableAsync($"`{_tableName}`");

            results
                .AssertColumnsExist("Id", "Name");
        }

        [TestMethod]
        public async Task SpecialCharacterTable_CanQuery_WithSpaces_Success()
        {
            QueryResult results = await TestRunner.ExecuteTableAsync($".   `{_tableName}`");

            results
                .AssertColumnsExist("Id", "Name");
        }

        [TestMethod]
        public async Task SpecialCharacterTable_CanQuery_UnequalBrackets_Success()
        {
            QueryResult results = await TestRunner.ExecuteTableAsync($"{_tableName}`");

            results
                .AssertColumnsExist("Id", "Name");
        }

        #endregion

        #region Can Insert

        [TestMethod]
        public async Task SpecialCharacterTable_CanInsert_NoBrackets_Success()
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
        public async Task SpecialCharacterTable_CanInsert_TableBrackets_Success()
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
        public async Task SpecialCharacterTable_CanInsert_WithSpaces_Success()
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
        public async Task SpecialCharacterTable_CanInsert_UnequalBrackets_Success()
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

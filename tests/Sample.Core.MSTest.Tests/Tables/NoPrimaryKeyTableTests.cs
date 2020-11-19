using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DBConfirm.Core.DataResults;
using System.Threading.Tasks;
using DBConfirm.Core.Data;
using DBConfirm.Core.Parameters;
using DBConfirm.Packages.SQLServer.MSTest;

namespace Sample.Core.MSTest.Tests.Tables
{
    [TestClass]
    public class NoPrimaryKeyTableTests : MSTestBase
    {
        private const string _tableName = "dbo.NoPrimaryKeyTable";

        #region Can Query
        [TestMethod]
        public async Task NoPrimaryKeyTable_CanQuery_Success()
        {
            QueryResult results = await TestRunner.ExecuteTableAsync(_tableName);

            results
                .AssertColumnsExist("Id");
        }

        #endregion

        #region Can Insert

        [TestMethod]
        public async Task NoPrimaryKeyTable_CanInsertDefaultData()
        {
            await TestRunner.InsertDefaultAsync(_tableName);

            QueryResult results = await TestRunner.ExecuteTableAsync(_tableName);

            results
                .AssertRowCount(1)
                .AssertColumnExists("Id");
        }

        [TestMethod]
        public async Task NoPrimaryKeyTable_CanInsertDefaultDataWithEmptyDataSetRow()
        {
            await TestRunner.InsertDataAsync(_tableName, new DataSetRow());

            QueryResult results = await TestRunner.ExecuteTableAsync(_tableName);

            results
                .AssertRowCount(1)
                .AssertColumnExists("Id")
                .AssertValue(0, "Id", Comparisons.IsNull());
        }

        [TestMethod]
        public async Task NoPrimaryKeyTable_CanInsertDefaultData_Twice_BothNull()
        {
            await TestRunner.InsertDefaultAsync(_tableName);
            await TestRunner.InsertDefaultAsync(_tableName);

            QueryResult results = await TestRunner.ExecuteTableAsync(_tableName);

            results
                .AssertRowCount(2)
                .AssertColumnExists("Id")
                .AssertValue(0, "Id", Comparisons.IsNull())
                .AssertValue(1, "Id", Comparisons.IsNull());
        }

        [TestMethod]
        public async Task NoPrimaryKeyTable_InsertIdentityManually_DataSet()
        {
            DataSetRow data1 = await TestRunner.InsertDataAsync(_tableName, new DataSetRow
            {
                ["Id"] = 2004
            });

            QueryResult results = await TestRunner.ExecuteTableAsync(_tableName);

            results
                .AssertRowCount(1)
                .AssertValue(0, "Id", 2004);

            Assert.AreEqual(2004, data1["Id"]);
        }

        #endregion
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using DBConfirm.Core.DataResults;
using System.Threading.Tasks;
using DBConfirm.Core.Data;
using DBConfirm.Packages.MySQL.MSTest;

namespace Sample.Core.MySQL.MSTest.Tests.Tables
{
    [TestClass]
    public class IdentityOnlyTableTests : MSTestBase
    {
        private const string _tableName = "IdentityOnlyTable";

        #region Can Query
        [TestMethod]
        public async Task IdentityOnlyTable_CanQuery_Success()
        {
            QueryResult results = await TestRunner.ExecuteTableAsync(_tableName);

            results
                .AssertColumnsExist("Id");
        }

        #endregion

        #region Can Insert

        [TestMethod]
        public async Task IdentityOnlyTable_CanInsertDefaultData()
        {
            await TestRunner.InsertDefaultAsync(_tableName);

            QueryResult results = await TestRunner.ExecuteTableAsync(_tableName);

            results
                .AssertRowCount(1)
                .AssertColumnExists("Id");
        }

        [TestMethod]
        public async Task IdentityOnlyTable_CanInsertDefaultDataWithEmptyDataSetRow()
        {
            await TestRunner.InsertDataAsync(_tableName, new DataSetRow());

            QueryResult results = await TestRunner.ExecuteTableAsync(_tableName);

            results
                .AssertRowCount(1)
                .AssertColumnExists("Id");
        }

        [TestMethod]
        public async Task IdentityOnlyTable_CanInsertDefaultData_Twice_DifferentIds()
        {
            DataSetRow data1 = await TestRunner.InsertDefaultAsync(_tableName);
            DataSetRow data2 = await TestRunner.InsertDefaultAsync(_tableName);

            QueryResult results = await TestRunner.ExecuteTableAsync(_tableName);

            results
                .AssertRowCount(2)
                .AssertColumnExists("Id")
                .AssertValue(0, "Id", data1["Id"])
                .AssertValue(1, "Id", data2["Id"]);

            Assert.AreNotEqual(data1["Id"], data2["Id"]);
        }

        [TestMethod]
        public async Task IdentityOnlyTable_InsertIdentityManually_DataSet()
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

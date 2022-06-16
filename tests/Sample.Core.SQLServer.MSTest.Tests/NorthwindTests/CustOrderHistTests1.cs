using Microsoft.VisualStudio.TestTools.UnitTesting;
using DBConfirm.Core.Data;
using DBConfirm.Core.DataResults;
using DBConfirm.Packages.SQLServer.MSTest;
using System.Threading.Tasks;
using DBConfirm.Core.Attributes;

namespace Sample.Core.SQLServer.MSTest.Tests.NorthwindTests
{
    [ConnectionStringName("NorthwindConnection")]
    [TestClass]
    public class CustOrderHistTests1 : MSTestBase
    {
        [TestMethod]
        public async Task NoData_ReturnNoRows()
        {
            QueryResult data = await TestRunner.ExecuteStoredProcedureQueryAsync("dbo.CustOrderHist", new DataSetRow
            {
                ["CustomerID"] = 123
            });

            data
                .AssertRowCount(0)
                .AssertColumnsExist("ProductName", "Total");
        }
    }
}

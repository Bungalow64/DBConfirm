using Microsoft.VisualStudio.TestTools.UnitTesting;
using DBConfirm.Core.Data;
using DBConfirm.Core.DataResults;
using DBConfirm.Packages.SQLServer.MSTest;
using System.Threading.Tasks;

namespace Sample.Core.MSTest.Tests.NorthwindTests;

[TestClass]
public class CustOrderHistTests2 : MSTestBase
{
    protected override string ParameterName => "NorthwindConnection";

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

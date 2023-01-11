using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sample.MSTest.Northwind.Tests.Templates;
using DBConfirm.Core.DataResults;
using DBConfirm.Packages.SQLServer.MSTest;
using System.Threading.Tasks;

namespace Sample.MSTest.Northwind.Tests.Procedures
{
    [TestClass]
    public class TenMostExpensiveProductsTests : MSTestBase
    {
        [TestMethod]
        public async Task NoData_ReturnNoRows()
        {
            QueryResult data = await TestRunner.ExecuteStoredProcedureQueryAsync("dbo.[Ten Most Expensive Products]");

            data
                .AssertRowCount(0)
                .AssertColumnsExist("TenMostExpensiveProducts", "UnitPrice");
        }

        [TestMethod]
        public async Task MultipleProducts_ShownInCorrectOrder()
        {
            await TestRunner.InsertTemplateAsync(new ProductsTemplate().WithUnitPrice(100).WithProductName("Product100"));
            await TestRunner.InsertTemplateAsync(new ProductsTemplate().WithUnitPrice(200).WithProductName("Product200"));
            await TestRunner.InsertTemplateAsync(new ProductsTemplate().WithUnitPrice(50).WithProductName("Product50"));

            QueryResult data = await TestRunner.ExecuteStoredProcedureQueryAsync("dbo.[Ten Most Expensive Products]");

            data
                .AssertRowCount(3)
                .AssertColumnsExist("TenMostExpensiveProducts", "UnitPrice")
                .AssertValue(0, "TenMostExpensiveProducts", "Product200")
                .AssertValue(1, "TenMostExpensiveProducts", "Product100")
                .AssertValue(2, "TenMostExpensiveProducts", "Product50");
        }

        [TestMethod]
        public async Task Has12Products_ShowTop10Only()
        {
            for (int x = 1; x <= 12; x++)
            {
                await TestRunner.InsertTemplateAsync(new ProductsTemplate().WithUnitPrice(x).WithProductName($"Product{x}"));
            }

            QueryResult data = await TestRunner.ExecuteStoredProcedureQueryAsync("dbo.[Ten Most Expensive Products]");

            data
                .AssertRowCount(10)
                .AssertColumnsExist("TenMostExpensiveProducts", "UnitPrice")
                .AssertValue(0, "TenMostExpensiveProducts", "Product12")
                .AssertValue(1, "TenMostExpensiveProducts", "Product11")
                .AssertValue(2, "TenMostExpensiveProducts", "Product10")
                .AssertValue(3, "TenMostExpensiveProducts", "Product9")
                .AssertValue(4, "TenMostExpensiveProducts", "Product8")
                .AssertValue(5, "TenMostExpensiveProducts", "Product7")
                .AssertValue(6, "TenMostExpensiveProducts", "Product6")
                .AssertValue(7, "TenMostExpensiveProducts", "Product5")
                .AssertValue(8, "TenMostExpensiveProducts", "Product4")
                .AssertValue(9, "TenMostExpensiveProducts", "Product3");
        }
    }
}

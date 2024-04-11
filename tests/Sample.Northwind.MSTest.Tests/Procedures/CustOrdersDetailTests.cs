using DBConfirm.Core.Data;
using DBConfirm.Core.DataResults;
using DBConfirm.Packages.SQLServer.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sample.Northwind.MSTest.Tests.Templates;
using Sample.Northwind.MSTest.Tests.Templates.Complex;
using System.Threading.Tasks;

namespace Sample.Northwind.MSTest.Tests.Procedures;

[TestClass]
public class CustOrdersDetailTests : MSTestBase
{
    [TestMethod]
    public async Task NoData_ReturnNoRows()
    {
        QueryResult data = await TestRunner.ExecuteStoredProcedureQueryAsync("dbo.CustOrdersDetail", new DataSetRow
        {
            ["OrderID"] = 123
        });

        data
            .AssertRowCount(0)
            .AssertColumnsExist("ProductName", "UnitPrice", "Quantity", "Discount", "ExtendedPrice");
    }

    [TestMethod]
    public async Task SingleOrder_ReturnOrderDetails()
    {
        CompleteOrderForCustomerTemplate order = await TestRunner.InsertTemplateAsync(new CompleteOrderForCustomerTemplate
        {
            ProductsTemplate = new ProductsTemplate().WithProductName("Product1"),
            Order_DetailsTemplate = new Order_DetailsTemplate().WithQuantity(5).WithUnitPrice(10.5m).WithDiscount(0.10f)
        });

        QueryResult data = await TestRunner.ExecuteStoredProcedureQueryAsync("dbo.CustOrdersDetail", new DataSetRow
        {
            ["OrderID"] = order.OrdersTemplate.Identity
        });

        data
            .AssertRowCount(1)
            .AssertColumnsExist("ProductName", "UnitPrice", "Quantity", "Discount", "ExtendedPrice")
            .AssertRowValues(0, new DataSetRow
            {
                ["ProductName"] = "Product1",
                ["UnitPrice"] = 10.5m,
                ["Quantity"] = (short)5,
                ["Discount"] = 10,
                ["ExtendedPrice"] = 47.25m
            });
    }

    [TestMethod]
    public async Task SingleOrder_UsingNumericalMatching_ReturnOrderDetails()
    {
        CompleteOrderForCustomerTemplate order = await TestRunner.InsertTemplateAsync(new CompleteOrderForCustomerTemplate
        {
            ProductsTemplate = new ProductsTemplate().WithProductName("Product1"),
            Order_DetailsTemplate = new Order_DetailsTemplate().WithQuantity(5).WithUnitPrice(10.5m).WithDiscount(0.10f)
        });

        QueryResult data = await TestRunner.ExecuteStoredProcedureQueryAsync("dbo.CustOrdersDetail", new DataSetRow
        {
            ["OrderID"] = order.OrdersTemplate.Identity
        });

        data
            .AssertRowCount(1)
            .AssertColumnsExist("ProductName", "UnitPrice", "Quantity", "Discount", "ExtendedPrice")
            .AssertRowValues(0, new DataSetRow
            {
                ["ProductName"] = "Product1",
                ["UnitPrice"] = Comparisons.MatchesNumber(10.5),
                ["Quantity"] = Comparisons.MatchesNumber(5),
                ["Discount"] = Comparisons.MatchesNumber(10),
                ["ExtendedPrice"] = Comparisons.MatchesNumber(47.25)
            });
    }

    [TestMethod]
    public async Task SingleOrder_NoDiscount_ReturnOrderDetails()
    {
        CompleteOrderForCustomerTemplate order = await TestRunner.InsertTemplateAsync(new CompleteOrderForCustomerTemplate
        {
            ProductsTemplate = new ProductsTemplate().WithProductName("Product1"),
            Order_DetailsTemplate = new Order_DetailsTemplate().WithQuantity(5).WithUnitPrice(10.5m)
        });

        QueryResult data = await TestRunner.ExecuteStoredProcedureQueryAsync("dbo.CustOrdersDetail", new DataSetRow
        {
            ["OrderID"] = order.OrdersTemplate.Identity
        });

        data
            .AssertRowCount(1)
            .AssertColumnsExist("ProductName", "UnitPrice", "Quantity", "Discount", "ExtendedPrice")
            .AssertRowValues(0, new DataSetRow
            {
                ["ProductName"] = "Product1",
                ["UnitPrice"] = 10.5m,
                ["Quantity"] = (short)5,
                ["Discount"] = 0,
                ["ExtendedPrice"] = 52.5m
            });
    }
}

using Sample.XUnit.Northwind.Tests.Templates;
using Sample.XUnit.Northwind.Tests.Templates.Complex;
using DBConfirm.Core.Data;
using DBConfirm.Core.DataResults;
using DBConfirm.Packages.SQLServer.XUnit;
using System.Threading.Tasks;
using Xunit;

namespace Sample.XUnit.Northwind.Tests.Procedures;

public class CustOrdersDetailTests : XUnitBase
{
    [Fact]
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

    [Fact]
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

    [Fact]
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

    [Fact]
    public async Task SingleOrder_33Discount_ReturnOrderDetails()
    {
        CompleteOrderForCustomerTemplate order = await TestRunner.InsertTemplateAsync(new CompleteOrderForCustomerTemplate
        {
            ProductsTemplate = new ProductsTemplate().WithProductName("Product1"),
            Order_DetailsTemplate = new Order_DetailsTemplate().WithQuantity(1).WithDiscount(0.33f).WithUnitPrice(10.5m)
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
                ["Quantity"] = (short)1,
                ["Discount"] = 33,
                ["ExtendedPrice"] = 7.04m
            });
    }
}

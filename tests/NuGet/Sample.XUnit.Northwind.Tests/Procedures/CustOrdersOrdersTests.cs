using Sample.XUnit.Northwind.Tests.Templates;
using Sample.XUnit.Northwind.Tests.Templates.Complex;
using DBConfirm.Core.Data;
using DBConfirm.Core.DataResults;
using DBConfirm.Packages.SQLServer.XUnit;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Sample.XUnit.Northwind.Tests.Procedures;

public class CustOrdersOrdersTests : XUnitBase
{
    [Fact]
    public async Task NoData_ReturnNoRows()
    {
        QueryResult data = await TestRunner.ExecuteStoredProcedureQueryAsync("dbo.CustOrdersOrders", new DataSetRow
        {
            ["CustomerID"] = 123
        });

        data
            .AssertRowCount(0)
            .AssertColumnsExist("OrderID", "OrderDate", "RequiredDate", "ShippedDate");
    }

    [Fact]
    public async Task SingleOrder_ReturnOrderDetails()
    {
        CompleteOrderForCustomerTemplate order = await TestRunner.InsertTemplateAsync(new CompleteOrderForCustomerTemplate
        {
            OrdersTemplate = new OrdersTemplate()
                .WithOrderID(1001)
                .WithOrderDate(DateTime.Parse("01-Mar-2020"))
                .WithRequiredDate(DateTime.Parse("02-Mar-2020"))
                .WithShippedDate(DateTime.Parse("03-Mar-2020")),
            Order_DetailsTemplate = new Order_DetailsTemplate().WithQuantity(5)
        });

        QueryResult data = await TestRunner.ExecuteStoredProcedureQueryAsync("dbo.CustOrdersOrders", new DataSetRow
        {
            ["CustomerID"] = order.CustomersTemplate.MergedData["CustomerID"]
        });

        data
            .AssertRowCount(1)
            .AssertColumnsExist("OrderID", "OrderDate", "RequiredDate", "ShippedDate")
            .AssertRowValues(0, new DataSetRow
            {
                ["OrderID"] = 1001,
                ["OrderDate"] = DateTime.Parse("01-Mar-2020"),
                ["RequiredDate"] = DateTime.Parse("02-Mar-2020"),
                ["ShippedDate"] = DateTime.Parse("03-Mar-2020")
            });
    }

    [Fact]
    public async Task SingleOrder_DefaultDates()
    {
        CompleteOrderForCustomerTemplate order = await TestRunner.InsertTemplateAsync(new CompleteOrderForCustomerTemplate
        {
            OrdersTemplate = new OrdersTemplate()
                .WithOrderID(1001),
            Order_DetailsTemplate = new Order_DetailsTemplate().WithQuantity(5)
        });

        QueryResult data = await TestRunner.ExecuteStoredProcedureQueryAsync("dbo.CustOrdersOrders", new DataSetRow
        {
            ["CustomerID"] = order.CustomersTemplate.MergedData["CustomerID"]
        });

        data
            .AssertRowCount(1)
            .AssertColumnsExist("OrderID", "OrderDate", "RequiredDate", "ShippedDate")
            .AssertRowValues(0, new DataSetRow
            {
                ["OrderID"] = 1001,
                ["OrderDate"] = null,
                ["RequiredDate"] = null,
                ["ShippedDate"] = null
            });
    }
}

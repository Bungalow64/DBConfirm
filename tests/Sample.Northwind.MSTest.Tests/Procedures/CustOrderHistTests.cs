using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sample.Northwind.MSTest.Tests.Templates;
using DBConfirm.Core.Data;
using DBConfirm.Core.DataResults;
using DBConfirm.Packages.SQLServer.MSTest;
using System.Threading.Tasks;
using Sample.Northwind.MSTest.Tests.Templates.Complex;

namespace Sample.Northwind.MSTest.Tests.Procedures;

[TestClass]
public class CustOrderHistTests : MSTestBase
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

    [TestMethod]
    public async Task SingleOrder_ReturnOrderDetails()
    {
        CompleteOrderForCustomerTemplate order = await TestRunner.InsertTemplateAsync(new CompleteOrderForCustomerTemplate
        {
            ProductsTemplate = new ProductsTemplate().WithProductName("Product1"),
            Order_DetailsTemplate = new Order_DetailsTemplate().WithQuantity(5)
        });

        QueryResult data = await TestRunner.ExecuteStoredProcedureQueryAsync("dbo.CustOrderHist", new DataSetRow
        {
            ["CustomerID"] = order.CustomersTemplate.MergedData["CustomerID"]
        });

        data
            .AssertRowCount(1)
            .AssertColumnsExist("ProductName", "Total")
            .AssertRowValues(0, new DataSetRow
            {
                ["ProductName"] = "Product1",
                ["Total"] = 5
            });
    }

    [TestMethod]
    public async Task SingleOrder_ForDifferentCustomer_ReturnNothing()
    {
        await TestRunner.InsertTemplateAsync(new CompleteOrderForCustomerTemplate
        {
            ProductsTemplate = new ProductsTemplate().WithProductName("Product1"),
            Order_DetailsTemplate = new Order_DetailsTemplate().WithQuantity(5)
        });

        QueryResult data = await TestRunner.ExecuteStoredProcedureQueryAsync("dbo.CustOrderHist", new DataSetRow
        {
            ["CustomerID"] = 100
        });

        data
            .AssertRowCount(0);
    }

    [TestMethod]
    public async Task TwoOrders_SameProduct_ReturnOneRow()
    {
        CompleteOrderForCustomerTemplate order1 = await TestRunner.InsertTemplateAsync(new CompleteOrderForCustomerTemplate
        {
            ProductsTemplate = new ProductsTemplate().WithProductName("Product1"),
            Order_DetailsTemplate = new Order_DetailsTemplate().WithQuantity(5)
        });

        await TestRunner.InsertTemplateAsync(new CompleteOrderForCustomerTemplate
        {
            CustomersTemplate = order1.CustomersTemplate,
            ProductsTemplate = order1.ProductsTemplate,
            Order_DetailsTemplate = new Order_DetailsTemplate().WithQuantity(8)
        });

        QueryResult data = await TestRunner.ExecuteStoredProcedureQueryAsync("dbo.CustOrderHist", new DataSetRow
        {
            ["CustomerID"] = order1.CustomersTemplate.MergedData["CustomerID"]
        });

        data
            .AssertRowCount(1)
            .AssertColumnsExist("ProductName", "Total")
            .AssertRowValues(0, new DataSetRow
            {
                ["ProductName"] = "Product1",
                ["Total"] = 13
            });
    }

    [TestMethod]
    public async Task TwoOrders_DifferentProduct_ReturnTwoRows()
    {
        CompleteOrderForCustomerTemplate order1 = await TestRunner.InsertTemplateAsync(new CompleteOrderForCustomerTemplate
        {
            ProductsTemplate = new ProductsTemplate().WithProductName("Product1"),
            Order_DetailsTemplate = new Order_DetailsTemplate().WithQuantity(5)
        });

        await TestRunner.InsertTemplateAsync(new CompleteOrderForCustomerTemplate
        {
            CustomersTemplate = order1.CustomersTemplate,
            ProductsTemplate = new ProductsTemplate().WithProductName("Product2"),
            Order_DetailsTemplate = new Order_DetailsTemplate().WithQuantity(8)
        });

        QueryResult data = await TestRunner.ExecuteStoredProcedureQueryAsync("dbo.CustOrderHist", new DataSetRow
        {
            ["CustomerID"] = order1.CustomersTemplate.MergedData["CustomerID"]
        });

        data
            .AssertRowCount(2)
            .AssertColumnsExist("ProductName", "Total")
            .AssertRowValues(0, new DataSetRow
            {
                ["ProductName"] = "Product1",
                ["Total"] = 5
            })
            .AssertRowValues(1, new DataSetRow
            {
                ["ProductName"] = "Product2",
                ["Total"] = 8
            });
    }
}

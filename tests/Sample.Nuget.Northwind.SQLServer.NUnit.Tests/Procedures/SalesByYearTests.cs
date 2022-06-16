using NUnit.Framework;
using Sample.Nuget.Northwind.SQLServer.NUnit.Tests.Templates;
using DBConfirm.Core.Data;
using DBConfirm.Core.DataResults;
using DBConfirm.Core.Parameters;
using DBConfirm.Packages.SQLServer.NUnit;
using System;
using System.Threading.Tasks;

namespace Sample.Nuget.Northwind.SQLServer.NUnit.Tests.Procedures
{
    [TestFixture]
    [NonParallelizable]
    public class SalesByYearTests : NUnitBase
    {
        [Test]
        public async Task NoData_ReturnNoRows()
        {
            QueryResult data = await TestRunner.ExecuteStoredProcedureQueryAsync("dbo.[Sales by Year]",
                new SqlQueryParameter("Beginning_Date", DateTime.Parse("01-Mar-2020")),
                new SqlQueryParameter("Ending_Date", DateTime.Parse("10-Mar-2020"))
                );

            data
                .AssertRowCount(0)
                .AssertColumnsExist("ShippedDate", "OrderID", "Subtotal", "Year");
        }

        [Test]
        public async Task OneOrder_ReturnOneRow()
        {
            await TestRunner.InsertTemplateAsync(new OrdersTemplate()
                .WithOrderID(1001)
                .WithShippedDate(DateTime.Parse("05-Mar-2020")));

            ProductsTemplate product = await TestRunner.InsertTemplateAsync<ProductsTemplate>();

            await TestRunner.InsertTemplateAsync(new Order_DetailsTemplate()
                .WithOrderID(1001)
                .WithProductID(product.Identity)
                .WithUnitPrice(10)
                .WithQuantity(5)
                .WithDiscount(0));

            QueryResult data = await TestRunner.ExecuteStoredProcedureQueryAsync("dbo.[Sales by Year]",
                new SqlQueryParameter("Beginning_Date", DateTime.Parse("01-Mar-2020")),
                new SqlQueryParameter("Ending_Date", DateTime.Parse("10-Mar-2020"))
                );

            data
                .AssertRowCount(1)
                .AssertRowValues(0, new DataSetRow
                {
                    { "ShippedDate", DateTime.Parse("05-Mar-2020") },
                    { "OrderID", 1001 },
                    { "Subtotal", 50m },
                    { "Year", "2020" }
                });
        }

        [Test]
        public async Task OneEmployeeAndOrder_OrderBeforeRange_ReturnNothing()
        {
            await TestRunner.InsertTemplateAsync(new OrdersTemplate()
                .WithOrderID(1001)
                .WithShippedDate(DateTime.Parse("05-Mar-2019")));

            var product = await TestRunner.InsertTemplateAsync<ProductsTemplate>();

            await TestRunner.InsertTemplateAsync(new Order_DetailsTemplate()
                .WithOrderID(1001)
                .WithProductID(product.Identity)
                .WithUnitPrice(10)
                .WithQuantity(5)
                .WithDiscount(0));

            QueryResult data = await TestRunner.ExecuteStoredProcedureQueryAsync("dbo.[Sales by Year]",
                new SqlQueryParameter("Beginning_Date", DateTime.Parse("01-Mar-2020")),
                new SqlQueryParameter("Ending_Date", DateTime.Parse("10-Mar-2020"))
                );

            data
                .AssertRowCount(0);
        }

        [Test]
        public async Task OneEmployeeAndOrder_OrderAfterRange_ReturnNothing()
        {
            await TestRunner.InsertTemplateAsync(new OrdersTemplate()
                .WithOrderID(1001)
                .WithShippedDate(DateTime.Parse("05-Sep-2020")));

            var product = await TestRunner.InsertTemplateAsync<ProductsTemplate>();

            await TestRunner.InsertTemplateAsync(new Order_DetailsTemplate()
                .WithOrderID(1001)
                .WithProductID(product.Identity)
                .WithUnitPrice(10)
                .WithQuantity(5)
                .WithDiscount(0));

            QueryResult data = await TestRunner.ExecuteStoredProcedureQueryAsync("dbo.[Sales by Year]",
                new SqlQueryParameter("Beginning_Date", DateTime.Parse("01-Mar-2020")),
                new SqlQueryParameter("Ending_Date", DateTime.Parse("10-Mar-2020"))
                );

            data
                .AssertRowCount(0);
        }

        [Test]
        public async Task TwoOrders_ReturnOneRowEach()
        {
            await TestRunner.InsertTemplateAsync(new OrdersTemplate()
                .WithOrderID(1001)
                .WithShippedDate(DateTime.Parse("05-Mar-2020")));

            await TestRunner.InsertTemplateAsync(new OrdersTemplate()
                .WithOrderID(1002)
                .WithShippedDate(DateTime.Parse("06-Mar-2020")));

            var product = await TestRunner.InsertTemplateAsync<ProductsTemplate>();

            await TestRunner.InsertTemplateAsync(new Order_DetailsTemplate()
                .WithOrderID(1001)
                .WithProductID(product.Identity)
                .WithUnitPrice(10)
                .WithQuantity(5)
                .WithDiscount(0));

            await TestRunner.InsertTemplateAsync(new Order_DetailsTemplate()
                .WithOrderID(1002)
                .WithProductID(product.Identity)
                .WithUnitPrice(20)
                .WithQuantity(5)
                .WithDiscount(0));

            QueryResult data = await TestRunner.ExecuteStoredProcedureQueryAsync("dbo.[Sales by Year]",
                new SqlQueryParameter("Beginning_Date", DateTime.Parse("01-Mar-2020")),
                new SqlQueryParameter("Ending_Date", DateTime.Parse("10-Mar-2020"))
                );

            data
                .AssertRowCount(2)
                .AssertRowValues(0, new DataSetRow
                {
                    { "ShippedDate", DateTime.Parse("05-Mar-2020") },
                    { "OrderID", 1001 },
                    { "Subtotal", 50m },
                    { "Year", "2020" }
                })
                .AssertRowValues(1, new DataSetRow
                {
                    { "ShippedDate", DateTime.Parse("06-Mar-2020") },
                    { "OrderID", 1002 },
                    { "Subtotal", 100m },
                    { "Year", "2020" }
                });
        }

        [Test]
        public async Task OneEmployeesAndOneOrderTwoLiness_ReturnOneRow()
        {
            await TestRunner.InsertTemplateAsync(new OrdersTemplate()
                .WithOrderID(1001)
                .WithShippedDate(DateTime.Parse("05-Mar-2020")));

            ProductsTemplate product1 = await TestRunner.InsertTemplateAsync<ProductsTemplate>();
            ProductsTemplate product2 = await TestRunner.InsertTemplateAsync<ProductsTemplate>();

            await TestRunner.InsertTemplateAsync(new Order_DetailsTemplate()
                .WithOrderID(1001)
                .WithProductID(product1.Identity)
                .WithUnitPrice(10)
                .WithQuantity(5)
                .WithDiscount(0));

            await TestRunner.InsertTemplateAsync(new Order_DetailsTemplate()
                .WithOrderID(1001)
                .WithProductID(product2.Identity)
                .WithUnitPrice(20)
                .WithQuantity(5)
                .WithDiscount(0));

            QueryResult data = await TestRunner.ExecuteStoredProcedureQueryAsync("dbo.[Sales by Year]",
                new SqlQueryParameter("Beginning_Date", DateTime.Parse("01-Mar-2020")),
                new SqlQueryParameter("Ending_Date", DateTime.Parse("10-Mar-2020"))
                );

            data
                .AssertRowCount(1)
                .AssertRowValues(0, new DataSetRow
                {
                    { "ShippedDate", DateTime.Parse("05-Mar-2020") },
                    { "OrderID", 1001 },
                    { "Subtotal", 150m },
                    { "Year", "2020" }
                });
        }

        [TestCase("0.0", 50)]
        [TestCase("0.1", 45)]
        [TestCase("0.2", 40)]
        [TestCase("0.3", 35)]
        [TestCase("0.4", 30)]
        [TestCase("0.5", 25)]
        [TestCase("0.6", 20)]
        [TestCase("0.7", 15)]
        [TestCase("0.8", 10)]
        [TestCase("0.9", 5)]
        [TestCase("1.0", 0)]
        public async Task OneOrderWithDiscount_ReturnValid(string discount, int expectedValue)
        {
            await TestRunner.InsertTemplateAsync(new OrdersTemplate()
                .WithOrderID(1001)
                .WithShippedDate(DateTime.Parse("05-Mar-2020")));

            ProductsTemplate product = await TestRunner.InsertTemplateAsync<ProductsTemplate>();

            await TestRunner.InsertTemplateAsync(new Order_DetailsTemplate()
                .WithOrderID(1001)
                .WithProductID(product.Identity)
                .WithUnitPrice(10)
                .WithQuantity(5)
                .WithDiscount(Convert.ToSingle(discount)));

            QueryResult data = await TestRunner.ExecuteStoredProcedureQueryAsync("dbo.[Sales by Year]",
                new SqlQueryParameter("Beginning_Date", DateTime.Parse("01-Mar-2020")),
                new SqlQueryParameter("Ending_Date", DateTime.Parse("10-Mar-2020"))
                );

            data
                .AssertRowCount(1)
                .AssertValue(0, "Subtotal", Convert.ToDecimal(expectedValue));
        }
    }
}

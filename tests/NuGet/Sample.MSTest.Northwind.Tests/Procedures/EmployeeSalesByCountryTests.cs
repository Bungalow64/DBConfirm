using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sample.MSTest.Northwind.Tests.Templates;
using DBConfirm.Core.Data;
using DBConfirm.Core.DataResults;
using DBConfirm.Core.Parameters;
using DBConfirm.Packages.SQLServer.MSTest;
using System;
using System.Threading.Tasks;

namespace Sample.MSTest.Northwind.Tests.Procedures
{
    [TestClass]
    public class EmployeeSalesByCountryTests : MSTestBase
    {
        [TestMethod]
        public async Task NoData_ReturnNoRows()
        {
            QueryResult data = await TestRunner.ExecuteStoredProcedureQueryAsync("dbo.[Employee Sales by Country]",
                new SqlQueryParameter("Beginning_Date", DateTime.Parse("01-Mar-2020")),
                new SqlQueryParameter("Ending_Date", DateTime.Parse("10-Mar-2020"))
                );

            data
                .AssertRowCount(0)
                .AssertColumnsExist("Country", "LastName", "FirstName", "ShippedDate", "OrderID", "SaleAmount");
        }

        [TestMethod]
        public async Task OneEmployeeAndOrder_ReturnOneRow()
        {
            EmployeesTemplate employee = await TestRunner.InsertTemplateAsync(new EmployeesTemplate()
                .WithFirstName("Jamie")
                .WithLastName("Burns")
                .WithCountry("UK"));

            await TestRunner.InsertTemplateAsync(new OrdersTemplate()
                .WithEmployeeID(employee.Identity)
                .WithOrderID(1001)
                .WithShippedDate(DateTime.Parse("05-Mar-2020")));

            var product = await TestRunner.InsertTemplateAsync<ProductsTemplate>();

            await TestRunner.InsertTemplateAsync(new Order_DetailsTemplate()
                .WithOrderID(1001)
                .WithProductID(product.Identity)
                .WithUnitPrice(10)
                .WithQuantity(5)
                .WithDiscount(0));

            QueryResult data = await TestRunner.ExecuteStoredProcedureQueryAsync("dbo.[Employee Sales by Country]",
                new SqlQueryParameter("Beginning_Date", DateTime.Parse("01-Mar-2020")),
                new SqlQueryParameter("Ending_Date", DateTime.Parse("10-Mar-2020"))
                );

            data
                .AssertRowCount(1)
                .AssertColumnsExist("Country", "LastName", "FirstName", "ShippedDate", "OrderID", "SaleAmount")
                .AssertRowValues(0, new DataSetRow
                {
                    { "Country", "UK" },
                    { "LastName", "Burns" },
                    { "FirstName", "Jamie" },
                    { "ShippedDate", DateTime.Parse("05-Mar-2020") },
                    { "OrderID", 1001 },
                    { "SaleAmount", 50m }
                });
        }

        [TestMethod]
        public async Task OneEmployeeAndOrder_OrderBeforeRange_ReturnNothing()
        {
            EmployeesTemplate employee = await TestRunner.InsertTemplateAsync(new EmployeesTemplate()
                .WithFirstName("Jamie")
                .WithLastName("Burns")
                .WithCountry("UK"));

            await TestRunner.InsertTemplateAsync(new OrdersTemplate()
                .WithEmployeeID(employee.Identity)
                .WithOrderID(1001)
                .WithShippedDate(DateTime.Parse("05-Mar-2019")));

            var product = await TestRunner.InsertTemplateAsync<ProductsTemplate>();

            await TestRunner.InsertTemplateAsync(new Order_DetailsTemplate()
                .WithOrderID(1001)
                .WithProductID(product.Identity)
                .WithUnitPrice(10)
                .WithQuantity(5)
                .WithDiscount(0));

            QueryResult data = await TestRunner.ExecuteStoredProcedureQueryAsync("dbo.[Employee Sales by Country]",
                new SqlQueryParameter("Beginning_Date", DateTime.Parse("01-Mar-2020")),
                new SqlQueryParameter("Ending_Date", DateTime.Parse("10-Mar-2020"))
                );

            data
                .AssertRowCount(0);
        }

        [TestMethod]
        public async Task OneEmployeeAndOrder_OrderAfterRange_ReturnNothing()
        {
            EmployeesTemplate employee = await TestRunner.InsertTemplateAsync(new EmployeesTemplate()
                .WithFirstName("Jamie")
                .WithLastName("Burns")
                .WithCountry("UK"));

            await TestRunner.InsertTemplateAsync(new OrdersTemplate()
                .WithEmployeeID(employee.Identity)
                .WithOrderID(1001)
                .WithShippedDate(DateTime.Parse("05-Sep-2020")));

            var product = await TestRunner.InsertTemplateAsync<ProductsTemplate>();

            await TestRunner.InsertTemplateAsync(new Order_DetailsTemplate()
                .WithOrderID(1001)
                .WithProductID(product.Identity)
                .WithUnitPrice(10)
                .WithQuantity(5)
                .WithDiscount(0));

            QueryResult data = await TestRunner.ExecuteStoredProcedureQueryAsync("dbo.[Employee Sales by Country]",
                new SqlQueryParameter("Beginning_Date", DateTime.Parse("01-Mar-2020")),
                new SqlQueryParameter("Ending_Date", DateTime.Parse("10-Mar-2020"))
                );

            data
                .AssertRowCount(0);
        }

        [TestMethod]
        public async Task TwoEmployeesAndOrder_ReturnOneRowEach()
        {
            EmployeesTemplate employee1 = await TestRunner.InsertTemplateAsync(new EmployeesTemplate()
                .WithFirstName("Jamie")
                .WithLastName("Burns")
                .WithCountry("UK"));

            EmployeesTemplate employee2 = await TestRunner.InsertTemplateAsync(new EmployeesTemplate()
                .WithFirstName("Jack")
                .WithLastName("Burns")
                .WithCountry("US"));

            await TestRunner.InsertTemplateAsync(new OrdersTemplate()
                .WithEmployeeID(employee1.Identity)
                .WithOrderID(1001)
                .WithShippedDate(DateTime.Parse("05-Mar-2020")));

            await TestRunner.InsertTemplateAsync(new OrdersTemplate()
                .WithEmployeeID(employee2.Identity)
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

            QueryResult data = await TestRunner.ExecuteStoredProcedureQueryAsync("dbo.[Employee Sales by Country]",
                new SqlQueryParameter("Beginning_Date", DateTime.Parse("01-Mar-2020")),
                new SqlQueryParameter("Ending_Date", DateTime.Parse("10-Mar-2020"))
                );

            data
                .AssertRowCount(2)
                .AssertRowValues(0, new DataSetRow
                {
                    { "Country", "UK" },
                    { "LastName", "Burns" },
                    { "FirstName", "Jamie" },
                    { "ShippedDate", DateTime.Parse("05-Mar-2020") },
                    { "OrderID", 1001 },
                    { "SaleAmount", 50m }
                })
                .AssertRowValues(1, new DataSetRow
                {
                    { "Country", "US" },
                    { "LastName", "Burns" },
                    { "FirstName", "Jack" },
                    { "ShippedDate", DateTime.Parse("06-Mar-2020") },
                    { "OrderID", 1002 },
                    { "SaleAmount", 100m }
                });
        }

        [TestMethod]
        public async Task OneEmployeesAndTwoOrders_ReturnTwoRows()
        {
            EmployeesTemplate employee = await TestRunner.InsertTemplateAsync(new EmployeesTemplate()
                .WithFirstName("Jamie")
                .WithLastName("Burns")
                .WithCountry("UK"));

            await TestRunner.InsertTemplateAsync(new OrdersTemplate()
                .WithEmployeeID(employee.Identity)
                .WithOrderID(1001)
                .WithShippedDate(DateTime.Parse("05-Mar-2020")));

            await TestRunner.InsertTemplateAsync(new OrdersTemplate()
                .WithEmployeeID(employee.Identity)
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

            QueryResult data = await TestRunner.ExecuteStoredProcedureQueryAsync("dbo.[Employee Sales by Country]",
                new SqlQueryParameter("Beginning_Date", DateTime.Parse("01-Mar-2020")),
                new SqlQueryParameter("Ending_Date", DateTime.Parse("10-Mar-2020"))
                );

            data
                .AssertRowCount(2)
                .AssertRowValues(0, new DataSetRow
                {
                    { "Country", "UK" },
                    { "LastName", "Burns" },
                    { "FirstName", "Jamie" },
                    { "ShippedDate", DateTime.Parse("05-Mar-2020") },
                    { "OrderID", 1001 },
                    { "SaleAmount", 50m }
                })
                .AssertRowValues(1, new DataSetRow
                {
                    { "Country", "UK" },
                    { "LastName", "Burns" },
                    { "FirstName", "Jamie" },
                    { "ShippedDate", DateTime.Parse("06-Mar-2020") },
                    { "OrderID", 1002 },
                    { "SaleAmount", 100m }
                });
        }

        [TestMethod]
        public async Task OneEmployeesAndOneOrderTwoLiness_ReturnOneRow()
        {
            EmployeesTemplate employee = await TestRunner.InsertTemplateAsync(new EmployeesTemplate()
                .WithFirstName("Jamie")
                .WithLastName("Burns")
                .WithCountry("UK"));

            await TestRunner.InsertTemplateAsync(new OrdersTemplate()
                .WithEmployeeID(employee.Identity)
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

            QueryResult data = await TestRunner.ExecuteStoredProcedureQueryAsync("dbo.[Employee Sales by Country]",
                new SqlQueryParameter("Beginning_Date", DateTime.Parse("01-Mar-2020")),
                new SqlQueryParameter("Ending_Date", DateTime.Parse("10-Mar-2020"))
                );

            data
                .AssertRowCount(1)
                .AssertRowValues(0, new DataSetRow
                {
                    { "Country", "UK" },
                    { "LastName", "Burns" },
                    { "FirstName", "Jamie" },
                    { "ShippedDate", DateTime.Parse("05-Mar-2020") },
                    { "OrderID", 1001 },
                    { "SaleAmount", 150m }
                });
        }
    }
}

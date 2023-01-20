using DBConfirm.Core.Data;
using DBConfirm.Core.DataResults;
using DBConfirm.Core.Parameters;
using DBConfirm.Packages.SQLServer.NUnit;
using NUnit.Framework;
using Sample.Northwind.NUnit.Tests.Templates;
using System;
using System.Threading.Tasks;

namespace Sample.Northwind.NUnit.Tests.Procedures
{
    public class SalesByCategoryTests : NUnitBase
    {
        [Test]
        public async Task NoData_ReturnNoRows()
        {
            QueryResult data = await TestRunner.ExecuteStoredProcedureQueryAsync("dbo.[SalesByCategory]",
                new SqlQueryParameter("CategoryName", "Category1"),
                new SqlQueryParameter("OrdYear", "1998")
                );

            data
                .AssertRowCount(0)
                .AssertColumnsExist("ProductName", "TotalPurchase");
        }

        [Test]
        public async Task OneOrder_ReturnOneRow()
        {
            CategoriesTemplate category = await TestRunner.InsertTemplateAsync(new CategoriesTemplate()
                .WithCategoryName("Category1"));

            ProductsTemplate product = await TestRunner.InsertTemplateAsync(new ProductsTemplate()
                .WithProductName("Product1")
                .WithCategoryID(category.Identity));

            await TestRunner.InsertTemplateAsync(new OrdersTemplate()
                .WithOrderID(1001)
                .WithOrderDate(DateTime.Parse("05-Mar-1998")));

            await TestRunner.InsertTemplateAsync(new Order_DetailsTemplate()
                .WithOrderID(1001)
                .WithProductID(product.Identity)
                .WithUnitPrice(10)
                .WithQuantity(5)
                .WithDiscount(0));

            QueryResult data = await TestRunner.ExecuteStoredProcedureQueryAsync("dbo.[SalesByCategory]",
                new SqlQueryParameter("CategoryName", "Category1"),
                new SqlQueryParameter("OrdYear", "1998")
                );

            data
                .AssertRowCount(1)
                .AssertRowValues(0, new DataSetRow
                {
                    { "ProductName", "Product1" },
                    { "TotalPurchase", 50m }
                });
        }

        [Test]
        public async Task OneOrderTwoLines_ReturnTwoRows()
        {
            CategoriesTemplate category = await TestRunner.InsertTemplateAsync(new CategoriesTemplate()
                .WithCategoryName("Category1"));

            ProductsTemplate product1 = await TestRunner.InsertTemplateAsync(new ProductsTemplate()
                .WithProductName("Product1")
                .WithCategoryID(category.Identity));

            ProductsTemplate product2 = await TestRunner.InsertTemplateAsync(new ProductsTemplate()
                .WithProductName("Product2")
                .WithCategoryID(category.Identity));

            await TestRunner.InsertTemplateAsync(new OrdersTemplate()
                .WithOrderID(1001)
                .WithOrderDate(DateTime.Parse("05-Mar-1998")));

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

            QueryResult data = await TestRunner.ExecuteStoredProcedureQueryAsync("dbo.[SalesByCategory]",
                new SqlQueryParameter("CategoryName", "Category1"),
                new SqlQueryParameter("OrdYear", "1998")
                );

            data
                .AssertRowCount(2)
                .AssertRowValues(0, new DataSetRow
                {
                    { "ProductName", "Product1" },
                    { "TotalPurchase", 50m }
                })
                .AssertRowValues(1, new DataSetRow
                {
                    { "ProductName", "Product2" },
                    { "TotalPurchase", 100m }
                });
        }

        [Test]
        public async Task TwoLinesForOneProduct_OneRow()
        {
            CategoriesTemplate category = await TestRunner.InsertTemplateAsync(new CategoriesTemplate()
                .WithCategoryName("Category1"));

            ProductsTemplate product1 = await TestRunner.InsertTemplateAsync(new ProductsTemplate()
                .WithProductName("Product1")
                .WithCategoryID(category.Identity));

            await TestRunner.InsertTemplateAsync(new OrdersTemplate()
                .WithOrderID(1001)
                .WithOrderDate(DateTime.Parse("05-Mar-1998")));

            await TestRunner.InsertTemplateAsync(new OrdersTemplate()
                .WithOrderID(1002)
                .WithOrderDate(DateTime.Parse("05-Mar-1998")));

            await TestRunner.InsertTemplateAsync(new Order_DetailsTemplate()
                .WithOrderID(1001)
                .WithProductID(product1.Identity)
                .WithUnitPrice(10)
                .WithQuantity(5)
                .WithDiscount(0));

            await TestRunner.InsertTemplateAsync(new Order_DetailsTemplate()
                .WithOrderID(1002)
                .WithProductID(product1.Identity)
                .WithUnitPrice(20)
                .WithQuantity(5)
                .WithDiscount(0));

            QueryResult data = await TestRunner.ExecuteStoredProcedureQueryAsync("dbo.[SalesByCategory]",
                new SqlQueryParameter("CategoryName", "Category1"),
                new SqlQueryParameter("OrdYear", "1998")
                );

            data
                .AssertRowCount(1)
                .AssertRowValues(0, new DataSetRow
                {
                    { "ProductName", "Product1" },
                    { "TotalPurchase", 150m }
                });
        }

        [Test]
        public async Task OneOrderForDifferentCategory_NoRows()
        {
            CategoriesTemplate category1 = await TestRunner.InsertTemplateAsync(new CategoriesTemplate()
                .WithCategoryName("Category1"));

            await TestRunner.InsertTemplateAsync(new CategoriesTemplate()
                .WithCategoryName("Category2"));

            ProductsTemplate product = await TestRunner.InsertTemplateAsync(new ProductsTemplate()
                .WithProductName("Product1")
                .WithCategoryID(category1.Identity));

            await TestRunner.InsertTemplateAsync(new OrdersTemplate()
                .WithOrderID(1001)
                .WithOrderDate(DateTime.Parse("05-Mar-1998")));

            await TestRunner.InsertTemplateAsync(new Order_DetailsTemplate()
                .WithOrderID(1001)
                .WithProductID(product.Identity)
                .WithUnitPrice(10)
                .WithQuantity(5)
                .WithDiscount(0));

            QueryResult data = await TestRunner.ExecuteStoredProcedureQueryAsync("dbo.[SalesByCategory]",
                new SqlQueryParameter("CategoryName", "Category2"),
                new SqlQueryParameter("OrdYear", "1998")
                );

            data
                .AssertRowCount(0);
        }

        [Test]
        public async Task OneOrderForDifferentYear_NoRows()
        {
            CategoriesTemplate category = await TestRunner.InsertTemplateAsync(new CategoriesTemplate()
                .WithCategoryName("Category1"));

            ProductsTemplate product = await TestRunner.InsertTemplateAsync(new ProductsTemplate()
                .WithProductName("Product1")
                .WithCategoryID(category.Identity));

            await TestRunner.InsertTemplateAsync(new OrdersTemplate()
                .WithOrderID(1001)
                .WithOrderDate(DateTime.Parse("05-Mar-1997")));

            await TestRunner.InsertTemplateAsync(new Order_DetailsTemplate()
                .WithOrderID(1001)
                .WithProductID(product.Identity)
                .WithUnitPrice(10)
                .WithQuantity(5)
                .WithDiscount(0));

            QueryResult data = await TestRunner.ExecuteStoredProcedureQueryAsync("dbo.[SalesByCategory]",
                new SqlQueryParameter("CategoryName", "Category2"),
                new SqlQueryParameter("OrdYear", "1998")
                );

            data
                .AssertRowCount(0);
        }

        [TestCase(1996, 100)]
        [TestCase(1997, 50)]
        [TestCase(1998, 80)]
        public async Task OneOrderFor1998Year_ValidValue(int year, int expectedTotal)
        {
            CategoriesTemplate category = await TestRunner.InsertTemplateAsync(new CategoriesTemplate()
                .WithCategoryName("Category1"));

            ProductsTemplate product = await TestRunner.InsertTemplateAsync(new ProductsTemplate()
                .WithProductName("Product1")
                .WithCategoryID(category.Identity));

            await TestRunner.InsertTemplateAsync(new OrdersTemplate()
                .WithOrderID(1001)
                .WithOrderDate(DateTime.Parse($"05-Mar-1996")));

            await TestRunner.InsertTemplateAsync(new OrdersTemplate()
                .WithOrderID(1002)
                .WithOrderDate(DateTime.Parse($"05-Mar-1997")));

            await TestRunner.InsertTemplateAsync(new OrdersTemplate()
                .WithOrderID(1003)
                .WithOrderDate(DateTime.Parse($"05-Mar-1998")));

            await TestRunner.InsertTemplateAsync(new Order_DetailsTemplate()
                .WithOrderID(1001)
                .WithProductID(product.Identity)
                .WithUnitPrice(10)
                .WithQuantity(10)
                .WithDiscount(0));

            await TestRunner.InsertTemplateAsync(new Order_DetailsTemplate()
                .WithOrderID(1002)
                .WithProductID(product.Identity)
                .WithUnitPrice(10)
                .WithQuantity(5)
                .WithDiscount(0));

            await TestRunner.InsertTemplateAsync(new Order_DetailsTemplate()
                .WithOrderID(1003)
                .WithProductID(product.Identity)
                .WithUnitPrice(10)
                .WithQuantity(8)
                .WithDiscount(0));

            QueryResult data = await TestRunner.ExecuteStoredProcedureQueryAsync("dbo.[SalesByCategory]",
                new SqlQueryParameter("CategoryName", "Category1"),
                new SqlQueryParameter("OrdYear", year)
                );

            data
                .AssertRowCount(1)
                .AssertValue(0, "TotalPurchase", Convert.ToDecimal(expectedTotal));
        }
    }
}

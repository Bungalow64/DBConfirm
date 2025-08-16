using DBConfirm.Core.DataResults;
using DBConfirm.Packages.SQLServer.XUnit;
using Sample.Northwind.XUnit.Tests.Templates;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Sample.Northwind.XUnit.Tests.Data;

public class UniquenessTests : XUnitBase
{
    [Fact]
    public async Task ProvideNoColumns_Failure()
    {
        await TestRunner.InsertTemplateAsync(new CategoriesTemplate()
            .WithCategoryName("Cat1")
            .WithDescription("Description1"));

        await TestRunner.InsertTemplateAsync(new CategoriesTemplate()
            .WithCategoryName("Cat1")
            .WithDescription("Description1"));

        QueryResult data = await TestRunner.ExecuteCommandAsync("SELECT Categories.CategoryName, Categories.Description FROM Categories ORDER BY Categories.CategoryID");

        try
        {
            data
                .AssertColumnCount(2)
                .AssertColumnValuesUnique();
        }
        catch (Exception ex)
        {
            Assert.Equal("No column names provided.  Specify columns to check for uniqueness", ex.Message);
            return;
        }

        Assert.Fail("Expected test to fail, but it passed");
    }

    [Fact]
    public async Task AllUnique_AssertColumnValuesUniqueTrue()
    {
        await TestRunner.InsertTemplateAsync(new CategoriesTemplate()
            .WithCategoryName("Cat1")
            .WithDescription("Description1"));

        await TestRunner.InsertTemplateAsync(new CategoriesTemplate()
            .WithCategoryName("Cat2")
            .WithDescription("Description2"));

        QueryResult data = await TestRunner.ExecuteCommandAsync("SELECT Categories.CategoryName, Categories.Description FROM Categories ORDER BY Categories.CategoryID");

        data
            .AssertColumnCount(2)
            .AssertColumnValuesUnique("CategoryName")
            .AssertColumnValuesUnique("Description")
            .AssertColumnValuesUnique("CategoryName", "Description");
    }

    [Fact]
    public async Task AllUnique_ProvideDuplicateNames_AssertColumnValuesUniqueTrue()
    {
        await TestRunner.InsertTemplateAsync(new CategoriesTemplate()
            .WithCategoryName("Cat1")
            .WithDescription("Description1"));

        await TestRunner.InsertTemplateAsync(new CategoriesTemplate()
            .WithCategoryName("Cat2")
            .WithDescription("Description2"));

        QueryResult data = await TestRunner.ExecuteCommandAsync("SELECT Categories.CategoryName, Categories.Description FROM Categories ORDER BY Categories.CategoryID");

        data
            .AssertColumnCount(2)
            .AssertColumnValuesUnique("CategoryName", "CategoryName")
            .AssertColumnValuesUnique("CategoryName", "CategoryName", "Description")
            .AssertColumnValuesUnique("CategoryName", "Description", "CategoryName", "Description");
    }

    [Fact]
    public async Task PartiallyUnique_AssertColumnValuesUniqueTrue()
    {
        await TestRunner.InsertTemplateAsync(new CategoriesTemplate()
            .WithCategoryName("Cat1")
            .WithDescription("Description1"));

        await TestRunner.InsertTemplateAsync(new CategoriesTemplate()
            .WithCategoryName("Cat1")
            .WithDescription("Description2"));

        await TestRunner.InsertTemplateAsync(new CategoriesTemplate()
            .WithCategoryName("Cat2")
            .WithDescription("Description3"));

        QueryResult data = await TestRunner.ExecuteCommandAsync("SELECT Categories.CategoryName, Categories.Description FROM Categories ORDER BY Categories.CategoryID");

        data
            .AssertColumnCount(2)
            .AssertColumnValuesUnique("Description")
            .AssertColumnValuesUnique("CategoryName", "Description");
    }

    [Fact]
    public async Task SingleColumnNotUnique_AssertColumnValuesUniqueFalse()
    {
        await TestRunner.InsertTemplateAsync(new CategoriesTemplate()
            .WithCategoryName("Cat2")
            .WithDescription("Description3"));

        await TestRunner.InsertTemplateAsync(new CategoriesTemplate()
            .WithCategoryName("Cat1")
            .WithDescription("Description1"));

        await TestRunner.InsertTemplateAsync(new CategoriesTemplate()
            .WithCategoryName("Cat1")
            .WithDescription("Description2"));

        QueryResult data = await TestRunner.ExecuteCommandAsync("SELECT Categories.CategoryName, Categories.Description FROM Categories ORDER BY Categories.CategoryID");

        try
        {
            data
                .AssertColumnCount(2)
                .AssertColumnValuesUnique("CategoryName");
        }
        catch (Exception ex)
        {
            Assert.Equal("Duplicate data found for column CategoryName in rows 1, 2", ex.Message);
            return;
        }

        Assert.Fail("Expected test to fail, but it passed");
    }

    [Fact]
    public async Task MultipleColumnsNotUnique_AssertColumnValuesUniqueFalse()
    {
        await TestRunner.InsertTemplateAsync(new CategoriesTemplate()
            .WithCategoryName("Cat1")
            .WithDescription("Description2"));

        await TestRunner.InsertTemplateAsync(new CategoriesTemplate()
            .WithCategoryName("Cat1")
            .WithDescription("Description1"));

        await TestRunner.InsertTemplateAsync(new CategoriesTemplate()
            .WithCategoryName("Cat1")
            .WithDescription("Description1"));

        QueryResult data = await TestRunner.ExecuteCommandAsync("SELECT Categories.CategoryName, Categories.Description FROM Categories ORDER BY Categories.CategoryID");

        try
        {
            data
                .AssertColumnCount(2)
                .AssertColumnValuesUnique("CategoryName", "Description");
        }
        catch (Exception ex)
        {
            Assert.Equal("Duplicate data found for columns CategoryName, Description in rows 1, 2", ex.Message);
            return;
        }

        Assert.Fail("Expected test to fail, but it passed");
    }

    [Theory]
    [InlineData("03-Mar-2023", "04-Mar-2023")]
    [InlineData("03-Mar-2023 09:23:23", "03-Mar-2023 09:23:24")]
    [InlineData("03-Mar-2023 09:23:23.001", "03-Mar-2023 09:23:23.002")]
    public async Task DateType_AllUnique_AssertColumnValuesUniqueTrue(string date1, string date2)
    {
        await TestRunner.InsertTemplateAsync(new EmployeesTemplate()
            .WithBirthDate(DateTime.Parse(date1))
            .WithCity("CityA"));

        await TestRunner.InsertTemplateAsync(new EmployeesTemplate()
            .WithBirthDate(DateTime.Parse(date2))
            .WithCity("CityB"));

        QueryResult data = await TestRunner.ExecuteCommandAsync("SELECT Employees.BirthDate, Employees.City FROM Employees ORDER BY Employees.EmployeeID");

        data
            .AssertColumnCount(2)
            .AssertColumnValuesUnique("BirthDate")
            .AssertColumnValuesUnique("City")
            .AssertColumnValuesUnique("City", "BirthDate");
    }

    [Fact]
    public async Task DateType_PartiallyUnique_AssertColumnValuesUniqueTrue()
    {
        await TestRunner.InsertTemplateAsync(new EmployeesTemplate()
            .WithBirthDate(DateTime.Parse("04-Mar-2023"))
            .WithCity("CityA"));

        await TestRunner.InsertTemplateAsync(new EmployeesTemplate()
            .WithBirthDate(DateTime.Parse("04-Mar-2023"))
            .WithCity("CityB"));

        QueryResult data = await TestRunner.ExecuteCommandAsync("SELECT Employees.BirthDate, Employees.City FROM Employees ORDER BY Employees.EmployeeID");

        data
            .AssertColumnCount(2)
            .AssertColumnValuesUnique("City", "BirthDate");
    }

    [Fact]
    public async Task DateType_SingleColumnNotUnique_AssertColumnValuesUniqueTrue()
    {
        await TestRunner.InsertTemplateAsync(new EmployeesTemplate()
            .WithBirthDate(DateTime.Parse("04-Mar-2023"))
            .WithCity("CityA"));

        await TestRunner.InsertTemplateAsync(new EmployeesTemplate()
            .WithBirthDate(DateTime.Parse("04-Mar-2023"))
            .WithCity("CityB"));

        QueryResult data = await TestRunner.ExecuteCommandAsync("SELECT Employees.BirthDate, Employees.City FROM Employees ORDER BY Employees.EmployeeID");

        try
        {
            data
                .AssertColumnCount(2)
                .AssertColumnValuesUnique("BirthDate");
        }
        catch (Exception ex)
        {
            Assert.Equal("Duplicate data found for column BirthDate in rows 0, 1", ex.Message);
            return;
        }

        Assert.Fail("Expected test to fail, but it passed");
    }

    [Fact]
    public async Task DateType_MultipleColumnsNotUnique_AssertColumnValuesUniqueTrue()
    {
        await TestRunner.InsertTemplateAsync(new EmployeesTemplate()
            .WithBirthDate(DateTime.Parse("04-Mar-2023"))
            .WithCity("CityB"));

        await TestRunner.InsertTemplateAsync(new EmployeesTemplate()
            .WithBirthDate(DateTime.Parse("04-Mar-2023"))
            .WithCity("CityA"));

        await TestRunner.InsertTemplateAsync(new EmployeesTemplate()
            .WithBirthDate(DateTime.Parse("04-Mar-2023"))
            .WithCity("CityA"));

        QueryResult data = await TestRunner.ExecuteCommandAsync("SELECT Employees.BirthDate, Employees.City FROM Employees ORDER BY Employees.EmployeeID");

        try
        {
            data
                .AssertColumnCount(2)
                .AssertColumnValuesUnique("BirthDate", "City");
        }
        catch (Exception ex)
        {
            Assert.Equal("Duplicate data found for columns BirthDate, City in rows 1, 2", ex.Message);
            return;
        }

        Assert.Fail("Expected test to fail, but it passed");
    }

    [Theory]
    [InlineData(1, 2)]
    [InlineData(10000, 10001)]
    public async Task IntType_AllUnique_AssertColumnValuesUniqueTrue(int value1, int value2)
    {
        var order = await TestRunner.InsertTemplateAsync<OrdersTemplate>();
        var product1 = await TestRunner.InsertTemplateAsync<ProductsTemplate>();
        var product2 = await TestRunner.InsertTemplateAsync<ProductsTemplate>();

        await TestRunner.InsertTemplateAsync(new Order_DetailsTemplate()
            .WithOrderID(order.Identity)
            .WithProductID(product1.Identity)
            .WithQuantity(value1));

        await TestRunner.InsertTemplateAsync(new Order_DetailsTemplate()
            .WithOrderID(order.Identity)
            .WithProductID(product2.Identity)
            .WithQuantity(value2));

        QueryResult data = await TestRunner.ExecuteCommandAsync("SELECT [Order Details].Quantity FROM [Order Details]");

        data
            .AssertColumnCount(1)
            .AssertColumnValuesUnique("Quantity");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(1001)]
    public async Task IntType_SingleColumnNotUnique_AssertColumnValuesUniqueTrue(int value)
    {
        var order = await TestRunner.InsertTemplateAsync<OrdersTemplate>();
        var product1 = await TestRunner.InsertTemplateAsync<ProductsTemplate>();
        var product2 = await TestRunner.InsertTemplateAsync<ProductsTemplate>();
        var product3 = await TestRunner.InsertTemplateAsync<ProductsTemplate>();

        await TestRunner.InsertTemplateAsync(new Order_DetailsTemplate()
            .WithOrderID(order.Identity)
            .WithProductID(product1.Identity)
            .WithQuantity(99));

        await TestRunner.InsertTemplateAsync(new Order_DetailsTemplate()
            .WithOrderID(order.Identity)
            .WithProductID(product2.Identity)
            .WithQuantity(value));

        await TestRunner.InsertTemplateAsync(new Order_DetailsTemplate()
            .WithOrderID(order.Identity)
            .WithProductID(product3.Identity)
            .WithQuantity(value));

        QueryResult data = await TestRunner.ExecuteCommandAsync("SELECT [Order Details].Quantity FROM [Order Details]");

        try
        {
            data
                .AssertColumnCount(1)
                .AssertColumnValuesUnique("Quantity");
        }
        catch (Exception ex)
        {
            Assert.Equal("Duplicate data found for column Quantity in rows 1, 2", ex.Message);
            return;
        }

        Assert.Fail("Expected test to fail, but it passed");
    }

    [Theory]
    [InlineData(1, 2)]
    [InlineData(10000, 10001)]
    [InlineData(1.01, 1.02)]
    public async Task DecimalType_AllUnique_AssertColumnValuesUniqueTrue(decimal value1, decimal value2)
    {
        var order = await TestRunner.InsertTemplateAsync<OrdersTemplate>();
        var product1 = await TestRunner.InsertTemplateAsync<ProductsTemplate>();
        var product2 = await TestRunner.InsertTemplateAsync<ProductsTemplate>();

        await TestRunner.InsertTemplateAsync(new Order_DetailsTemplate()
            .WithOrderID(order.Identity)
            .WithProductID(product1.Identity)
            .WithUnitPrice(value1));

        await TestRunner.InsertTemplateAsync(new Order_DetailsTemplate()
            .WithOrderID(order.Identity)
            .WithProductID(product2.Identity)
            .WithUnitPrice(value2));

        QueryResult data = await TestRunner.ExecuteCommandAsync("SELECT [Order Details].UnitPrice FROM [Order Details]");

        data
            .AssertColumnCount(1)
            .AssertColumnValuesUnique("UnitPrice");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(1001)]
    [InlineData(100.01)]
    public async Task DecimalType_SingleColumnNotUnique_AssertColumnValuesUniqueTrue(decimal value)
    {
        var order = await TestRunner.InsertTemplateAsync<OrdersTemplate>();
        var product1 = await TestRunner.InsertTemplateAsync<ProductsTemplate>();
        var product2 = await TestRunner.InsertTemplateAsync<ProductsTemplate>();
        var product3 = await TestRunner.InsertTemplateAsync<ProductsTemplate>();

        await TestRunner.InsertTemplateAsync(new Order_DetailsTemplate()
            .WithOrderID(order.Identity)
            .WithProductID(product1.Identity)
            .WithUnitPrice(99));

        await TestRunner.InsertTemplateAsync(new Order_DetailsTemplate()
            .WithOrderID(order.Identity)
            .WithProductID(product2.Identity)
            .WithUnitPrice(value));

        await TestRunner.InsertTemplateAsync(new Order_DetailsTemplate()
            .WithOrderID(order.Identity)
            .WithProductID(product3.Identity)
            .WithUnitPrice(value));

        QueryResult data = await TestRunner.ExecuteCommandAsync("SELECT [Order Details].UnitPrice FROM [Order Details]");

        try
        {
            data
                .AssertColumnCount(1)
                .AssertColumnValuesUnique("UnitPrice");
        }
        catch (Exception ex)
        {
            Assert.Equal("Duplicate data found for column UnitPrice in rows 1, 2", ex.Message);
            return;
        }

        Assert.Fail("Expected test to fail, but it passed");
    }

    [Theory]
    [InlineData(0f, 0.1f)]
    [InlineData(0.001f, 0.002f)]
    [InlineData(0.999f, 0.998f)]
    public async Task FloatType_AllUnique_AssertColumnValuesUniqueTrue(float value1, float value2)
    {
        var order = await TestRunner.InsertTemplateAsync<OrdersTemplate>();
        var product1 = await TestRunner.InsertTemplateAsync<ProductsTemplate>();
        var product2 = await TestRunner.InsertTemplateAsync<ProductsTemplate>();

        await TestRunner.InsertTemplateAsync(new Order_DetailsTemplate()
            .WithOrderID(order.Identity)
            .WithProductID(product1.Identity)
            .WithDiscount(value1));

        await TestRunner.InsertTemplateAsync(new Order_DetailsTemplate()
            .WithOrderID(order.Identity)
            .WithProductID(product2.Identity)
            .WithDiscount(value2));

        QueryResult data = await TestRunner.ExecuteCommandAsync("SELECT [Order Details].Discount FROM [Order Details]");

        data
            .AssertColumnCount(1)
            .AssertColumnValuesUnique("Discount");
    }

    [Theory]
    [InlineData(0f)]
    [InlineData(0.1f)]
    [InlineData(0.001f)]
    [InlineData(0.0001f)]
    [InlineData(0.999f)]
    public async Task FloatType_SingleColumnNotUnique_AssertColumnValuesUniqueTrue(float value)
    {
        var order = await TestRunner.InsertTemplateAsync<OrdersTemplate>();
        var product1 = await TestRunner.InsertTemplateAsync<ProductsTemplate>();
        var product2 = await TestRunner.InsertTemplateAsync<ProductsTemplate>();
        var product3 = await TestRunner.InsertTemplateAsync<ProductsTemplate>();

        await TestRunner.InsertTemplateAsync(new Order_DetailsTemplate()
            .WithOrderID(order.Identity)
            .WithProductID(product1.Identity)
            .WithDiscount(0.5f));

        await TestRunner.InsertTemplateAsync(new Order_DetailsTemplate()
            .WithOrderID(order.Identity)
            .WithProductID(product2.Identity)
            .WithDiscount(value));

        await TestRunner.InsertTemplateAsync(new Order_DetailsTemplate()
            .WithOrderID(order.Identity)
            .WithProductID(product3.Identity)
            .WithDiscount(value));

        QueryResult data = await TestRunner.ExecuteCommandAsync("SELECT [Order Details].Discount FROM [Order Details]");

        try
        {
            data
                .AssertColumnCount(1)
                .AssertColumnValuesUnique("Discount");
        }
        catch (Exception ex)
        {
            Assert.Equal("Duplicate data found for column Discount in rows 1, 2", ex.Message);
            return;
        }

        Assert.Fail("Expected test to fail, but it passed");
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    public async Task BoolType_AllUnique_AssertColumnValuesUniqueTrue(bool value1, bool value2)
    {
        await TestRunner.InsertTemplateAsync(new ProductsTemplate()
            .WithDiscontinued(value1));

        await TestRunner.InsertTemplateAsync(new ProductsTemplate()
            .WithDiscontinued(value2));

        QueryResult data = await TestRunner.ExecuteCommandAsync("SELECT [Products].Discontinued FROM [Products] ORDER BY [Products].ProductID");

        data
            .AssertColumnCount(1)
            .AssertColumnValuesUnique("Discontinued");
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task BoolType_SingleColumnNotUnique_AssertColumnValuesUniqueTrue(bool value)
    {
        await TestRunner.InsertTemplateAsync(new ProductsTemplate()
            .WithDiscontinued(!value));

        await TestRunner.InsertTemplateAsync(new ProductsTemplate()
            .WithDiscontinued(value));

        await TestRunner.InsertTemplateAsync(new ProductsTemplate()
            .WithDiscontinued(value));

        QueryResult data = await TestRunner.ExecuteCommandAsync("SELECT [Products].Discontinued FROM [Products] ORDER BY [Products].ProductID");

        try
        {
            data
                .AssertColumnCount(1)
                .AssertColumnValuesUnique("Discontinued");
        }
        catch (Exception ex)
        {
            Assert.Equal("Duplicate data found for column Discontinued in rows 1, 2", ex.Message);
            return;
        }

        Assert.Fail("Expected test to fail, but it passed");
    }

    [Theory]
    [InlineData("A", "B")]
    [InlineData("A", "")]
    [InlineData("", "A")]
    [InlineData("", " ")]
    [InlineData("  ", " ")]
    [InlineData("ValueA", "ValueB")]
    [InlineData("ValueA", "Valuea")]
    public async Task StringType_AllUnique_AssertColumnValuesUniqueTrue(string value1, string value2)
    {
        await TestRunner.InsertTemplateAsync(new CategoriesTemplate()
            .WithCategoryName(value1));

        await TestRunner.InsertTemplateAsync(new CategoriesTemplate()
            .WithCategoryName(value2));

        QueryResult data = await TestRunner.ExecuteCommandAsync("SELECT [Categories].CategoryName FROM [Categories] ORDER BY Categories.CategoryID");

        data
            .AssertColumnCount(1)
            .AssertColumnValuesUnique("CategoryName");
    }

    [Theory]
    [InlineData("A")]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("  ")]
    [InlineData("ValueA")]
    public async Task StringType_SingleColumnNotUnique_AssertColumnValuesUniqueTrue(string value1)
    {
        await TestRunner.InsertTemplateAsync(new CategoriesTemplate()
            .WithCategoryName("Cat A"));

        await TestRunner.InsertTemplateAsync(new CategoriesTemplate()
            .WithCategoryName(value1));

        await TestRunner.InsertTemplateAsync(new CategoriesTemplate()
            .WithCategoryName(value1));

        QueryResult data = await TestRunner.ExecuteCommandAsync("SELECT [Categories].CategoryName FROM [Categories] ORDER BY Categories.CategoryID");

        try
        {
            data
                .AssertColumnCount(1)
                .AssertColumnValuesUnique("CategoryName");
        }
        catch (Exception ex)
        {
            Assert.Equal("Duplicate data found for column CategoryName in rows 1, 2", ex.Message);
            return;
        }

        Assert.Fail("Expected test to fail, but it passed");
    }
}

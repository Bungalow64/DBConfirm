using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sample.MSTest.Northwind.Tests.Templates;
using Sample.MSTest.Northwind.Tests.Templates.Complex;
using SQLConfirm.Packages.SQLServer.MSTest;
using System.Threading.Tasks;

namespace Sample.MSTest.Northwind.Tests.Correctness
{
    [TestClass]
    public class TemplateTests : MSTestBase
    {
        [TestMethod]
        public async Task Templates_CanAllBeAdded()
        {
            await TestRunner.InsertTemplateAsync<CategoriesTemplate>();
            await TestRunner.InsertTemplateAsync(new CustomersTemplate().WithCustomerID("Cust1"));
            await TestRunner.InsertTemplateAsync(new CustomerDemographicsTemplate().WithCustomerTypeID("Type1"));
            await TestRunner.InsertTemplateAsync(new CustomerCustomerDemoTemplate().WithCustomerID("Cust1").WithCustomerTypeID("Type1"));

            EmployeesTemplate employee = await TestRunner.InsertTemplateAsync(new EmployeesTemplate());

            RegionTemplate region = await TestRunner.InsertTemplateAsync<RegionTemplate>();

            await TestRunner.InsertTemplateAsync(new TerritoriesTemplate().WithTerritoryID("Terry1").WithRegionID((int)region.MergedData["RegionID"]));

            await TestRunner.InsertTemplateAsync(new EmployeeTerritoriesTemplate()
                .WithEmployeeID(employee.IdentityResolver)
                .WithTerritoryID("Terry1")
                );

            await TestRunner.InsertTemplateAsync(new ProductsTemplate().WithProductID(3001));
            await TestRunner.InsertTemplateAsync(new OrdersTemplate().WithOrderID(2001));
            await TestRunner.InsertTemplateAsync(new Order_DetailsTemplate().WithOrderID(2001).WithProductID(3001));

            await TestRunner.InsertTemplateAsync<ShippersTemplate>();

            await TestRunner.InsertTemplateAsync<SuppliersTemplate>();
        }

        [TestMethod]
        public async Task Templates_Complex_CanAllBeAdded()
        {
            await TestRunner.InsertTemplateAsync<CompleteOrderForCustomerTemplate>();
        }
    }
}

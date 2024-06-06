﻿using NUnit.Framework;
using Sample.NUnit.Northwind.Tests.Templates;
using Sample.NUnit.Northwind.Tests.Templates.Complex;
using DBConfirm.Packages.SQLServer.NUnit;
using System.Threading.Tasks;

namespace Sample.NUnit.Northwind.Tests.Correctness;

[TestFixture]
[NonParallelizable]
public class TemplateTests : NUnitBase
{
    [Test]
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

    [Test]
    public async Task Templates_Complex_CanAllBeAdded()
    {
        await TestRunner.InsertTemplateAsync<CompleteOrderForCustomerTemplate>();
    }
}

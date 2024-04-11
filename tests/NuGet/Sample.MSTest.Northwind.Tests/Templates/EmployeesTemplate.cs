using System;
using DBConfirm.Core.Data;
using DBConfirm.Core.Templates;
using DBConfirm.Core.Templates.Abstract;

namespace Sample.MSTest.Northwind.Tests.Templates;

public class EmployeesTemplate: BaseIdentityTemplate<EmployeesTemplate>
{
    public override string TableName => "[dbo].[Employees]";
    
    public override string IdentityColumnName => "EmployeeID";

    public override DataSetRow DefaultData => new()
    {
        ["LastName"] = "SampleLastName",
        ["FirstName"] = "SampleFirs"
    };

    public EmployeesTemplate WithEmployeeID(int value) => SetValue("EmployeeID", value);
    public EmployeesTemplate WithLastName(string value) => SetValue("LastName", value);
    public EmployeesTemplate WithFirstName(string value) => SetValue("FirstName", value);
    public EmployeesTemplate WithTitle(string value) => SetValue("Title", value);
    public EmployeesTemplate WithTitleOfCourtesy(string value) => SetValue("TitleOfCourtesy", value);
    public EmployeesTemplate WithBirthDate(DateTime value) => SetValue("BirthDate", value);
    public EmployeesTemplate WithHireDate(DateTime value) => SetValue("HireDate", value);
    public EmployeesTemplate WithAddress(string value) => SetValue("Address", value);
    public EmployeesTemplate WithCity(string value) => SetValue("City", value);
    public EmployeesTemplate WithRegion(string value) => SetValue("Region", value);
    public EmployeesTemplate WithPostalCode(string value) => SetValue("PostalCode", value);
    public EmployeesTemplate WithCountry(string value) => SetValue("Country", value);
    public EmployeesTemplate WithHomePhone(string value) => SetValue("HomePhone", value);
    public EmployeesTemplate WithExtension(string value) => SetValue("Extension", value);
    public EmployeesTemplate WithPhoto(byte[] value) => SetValue("Photo", value);
    public EmployeesTemplate WithNotes(string value) => SetValue("Notes", value);
    public EmployeesTemplate WithReportsTo(int value) => SetValue("ReportsTo", value);
    public EmployeesTemplate WithReportsTo(IResolver resolver) => SetValue("ReportsTo", resolver);
    public EmployeesTemplate WithPhotoPath(string value) => SetValue("PhotoPath", value);
}
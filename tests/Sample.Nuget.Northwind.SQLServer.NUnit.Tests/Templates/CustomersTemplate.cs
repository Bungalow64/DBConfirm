using DBConfirm.Core.Data;
using DBConfirm.Core.Templates;

namespace Sample.Nuget.Northwind.SQLServer.NUnit.Tests.Templates
{
    public class CustomersTemplate: BaseSimpleTemplate<CustomersTemplate>
    {
        public override string TableName => "[dbo].[Customers]";
        
        public override DataSetRow DefaultData => new DataSetRow
        {
            ["CustomerID"] = "Sampl",
            ["CompanyName"] = "SampleCompanyName"
        };

        public CustomersTemplate WithCustomerID(string value) => SetValue("CustomerID", value);
        public CustomersTemplate WithCompanyName(string value) => SetValue("CompanyName", value);
        public CustomersTemplate WithContactName(string value) => SetValue("ContactName", value);
        public CustomersTemplate WithContactTitle(string value) => SetValue("ContactTitle", value);
        public CustomersTemplate WithAddress(string value) => SetValue("Address", value);
        public CustomersTemplate WithCity(string value) => SetValue("City", value);
        public CustomersTemplate WithRegion(string value) => SetValue("Region", value);
        public CustomersTemplate WithPostalCode(string value) => SetValue("PostalCode", value);
        public CustomersTemplate WithCountry(string value) => SetValue("Country", value);
        public CustomersTemplate WithPhone(string value) => SetValue("Phone", value);
        public CustomersTemplate WithFax(string value) => SetValue("Fax", value);
    }
}
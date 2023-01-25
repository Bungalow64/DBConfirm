using DBConfirm.Core.Data;
using DBConfirm.Core.Templates;

namespace Sample.Northwind.NUnit.Tests.Templates
{
    public class SuppliersTemplate : BaseIdentityTemplate<SuppliersTemplate>
    {
        public override string TableName => "[dbo].[Suppliers]";

        public override string IdentityColumnName => "SupplierID";

        public override DataSetRow DefaultData => new DataSetRow
        {
            ["CompanyName"] = "SampleCompanyName"
        };

        public SuppliersTemplate WithSupplierID(int value) => SetValue("SupplierID", value);
        public SuppliersTemplate WithCompanyName(string value) => SetValue("CompanyName", value);
        public SuppliersTemplate WithContactName(string value) => SetValue("ContactName", value);
        public SuppliersTemplate WithContactTitle(string value) => SetValue("ContactTitle", value);
        public SuppliersTemplate WithAddress(string value) => SetValue("Address", value);
        public SuppliersTemplate WithCity(string value) => SetValue("City", value);
        public SuppliersTemplate WithRegion(string value) => SetValue("Region", value);
        public SuppliersTemplate WithPostalCode(string value) => SetValue("PostalCode", value);
        public SuppliersTemplate WithCountry(string value) => SetValue("Country", value);
        public SuppliersTemplate WithPhone(string value) => SetValue("Phone", value);
        public SuppliersTemplate WithFax(string value) => SetValue("Fax", value);
        public SuppliersTemplate WithHomePage(string value) => SetValue("HomePage", value);
    }
}
using SQLConfirm.Core.Data;
using SQLConfirm.Core.Templates;
using SQLConfirm.Core.Templates.Abstract;

namespace Sample.MSTest.Northwind.Tests.Templates
{
    public class ProductsTemplate: BaseIdentityTemplate<ProductsTemplate>
    {
        public override string TableName => "[dbo].[Products]";
        
        public override string IdentityColumnName => "ProductID";

        public override DataSetRow DefaultData => new DataSetRow
        {
            ["ProductName"] = "SampleProductName"
        };

        public ProductsTemplate WithProductID(int value) => SetValue("ProductID", value);
        public ProductsTemplate WithProductName(string value) => SetValue("ProductName", value);
        public ProductsTemplate WithSupplierID(int value) => SetValue("SupplierID", value);
        public ProductsTemplate WithSupplierID(IResolver resolver) => SetValue("SupplierID", resolver);
        public ProductsTemplate WithCategoryID(int value) => SetValue("CategoryID", value);
        public ProductsTemplate WithCategoryID(IResolver resolver) => SetValue("CategoryID", resolver);
        public ProductsTemplate WithQuantityPerUnit(string value) => SetValue("QuantityPerUnit", value);
        public ProductsTemplate WithUnitPrice(decimal value) => SetValue("UnitPrice", value);
        public ProductsTemplate WithUnitsInStock(int value) => SetValue("UnitsInStock", value);
        public ProductsTemplate WithUnitsOnOrder(int value) => SetValue("UnitsOnOrder", value);
        public ProductsTemplate WithReorderLevel(int value) => SetValue("ReorderLevel", value);
        public ProductsTemplate WithDiscontinued(bool value) => SetValue("Discontinued", value);
    }
}
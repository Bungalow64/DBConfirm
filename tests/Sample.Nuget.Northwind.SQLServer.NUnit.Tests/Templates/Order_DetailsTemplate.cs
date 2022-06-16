using DBConfirm.Core.Data;
using DBConfirm.Core.Templates;
using DBConfirm.Core.Templates.Abstract;
using DBConfirm.Core.Templates.Placeholders;

namespace Sample.Nuget.Northwind.SQLServer.NUnit.Tests.Templates
{
    public class Order_DetailsTemplate: BaseSimpleTemplate<Order_DetailsTemplate>
    {
        public override string TableName => "[dbo].[Order Details]";
        
        public override DataSetRow DefaultData => new DataSetRow
        {
            ["OrderID"] = Placeholders.IsRequired(),
            ["ProductID"] = Placeholders.IsRequired()
        };

        public Order_DetailsTemplate WithOrderID(int value) => SetValue("OrderID", value);
        public Order_DetailsTemplate WithOrderID(IResolver resolver) => SetValue("OrderID", resolver);
        public Order_DetailsTemplate WithProductID(int value) => SetValue("ProductID", value);
        public Order_DetailsTemplate WithProductID(IResolver resolver) => SetValue("ProductID", resolver);
        public Order_DetailsTemplate WithUnitPrice(decimal value) => SetValue("UnitPrice", value);
        public Order_DetailsTemplate WithQuantity(int value) => SetValue("Quantity", value);
        public Order_DetailsTemplate WithDiscount(float value) => SetValue("Discount", value);
    }
}
using System;
using SQLConfirm.Core.Data;
using SQLConfirm.Core.Templates;
using SQLConfirm.Core.Templates.Abstract;

namespace Sample.MSTest.Northwind.Tests.Templates
{
    public class OrdersTemplate: BaseIdentityTemplate<OrdersTemplate>
    {
        public override string TableName => "[dbo].[Orders]";
        
        public override string IdentityColumnName => "OrderID";

        public override DataSetRow DefaultData => new DataSetRow
        {
            
        };

        public OrdersTemplate WithOrderID(int value) => SetValue("OrderID", value);
        public OrdersTemplate WithCustomerID(string value) => SetValue("CustomerID", value);
        public OrdersTemplate WithEmployeeID(int value) => SetValue("EmployeeID", value);
        public OrdersTemplate WithEmployeeID(IResolver resolver) => SetValue("EmployeeID", resolver);
        public OrdersTemplate WithOrderDate(DateTime value) => SetValue("OrderDate", value);
        public OrdersTemplate WithRequiredDate(DateTime value) => SetValue("RequiredDate", value);
        public OrdersTemplate WithShippedDate(DateTime value) => SetValue("ShippedDate", value);
        public OrdersTemplate WithShipVia(int value) => SetValue("ShipVia", value);
        public OrdersTemplate WithShipVia(IResolver resolver) => SetValue("ShipVia", resolver);
        public OrdersTemplate WithFreight(decimal value) => SetValue("Freight", value);
        public OrdersTemplate WithShipName(string value) => SetValue("ShipName", value);
        public OrdersTemplate WithShipAddress(string value) => SetValue("ShipAddress", value);
        public OrdersTemplate WithShipCity(string value) => SetValue("ShipCity", value);
        public OrdersTemplate WithShipRegion(string value) => SetValue("ShipRegion", value);
        public OrdersTemplate WithShipPostalCode(string value) => SetValue("ShipPostalCode", value);
        public OrdersTemplate WithShipCountry(string value) => SetValue("ShipCountry", value);
    }
}
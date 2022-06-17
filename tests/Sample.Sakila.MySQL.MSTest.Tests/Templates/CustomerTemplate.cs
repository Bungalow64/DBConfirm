using System;
using DBConfirm.Core.Data;
using DBConfirm.Core.Templates;
using DBConfirm.Core.Templates.Abstract;
using DBConfirm.Core.Templates.Placeholders;

namespace Sample.Sakila.MySQL.MSTest.Tests.Templates
{
    public class CustomerTemplate : BaseIdentityTemplate<CustomerTemplate>
    {
        public override string TableName => "`customer`";
        
        public override string IdentityColumnName => "customer_id";

        public override DataSetRow DefaultData => new DataSetRow
        {
            ["store_id"] = Placeholders.IsRequired(),
            ["first_name"] = "SampleFirst_name",
            ["last_name"] = "SampleLast_name",
            ["address_id"] = Placeholders.IsRequired(),
            ["create_date"] = DateTime.Parse("22-Oct-2020")
        };

        public CustomerTemplate WithCustomer_id(int value) => SetValue("customer_id", value);
        public CustomerTemplate WithStore_id(int value) => SetValue("store_id", value);
        public CustomerTemplate WithStore_id(IResolver resolver) => SetValue("store_id", resolver);
        public CustomerTemplate WithFirst_name(string value) => SetValue("first_name", value);
        public CustomerTemplate WithLast_name(string value) => SetValue("last_name", value);
        public CustomerTemplate WithEmail(string value) => SetValue("email", value);
        public CustomerTemplate WithAddress_id(int value) => SetValue("address_id", value);
        public CustomerTemplate WithAddress_id(IResolver resolver) => SetValue("address_id", resolver);
        public CustomerTemplate WithActive(bool value) => SetValue("active", value);
        public CustomerTemplate WithCreate_date(DateTime value) => SetValue("create_date", value);
        public CustomerTemplate WithLast_update(DateTime value) => SetValue("last_update", value);
    }
}
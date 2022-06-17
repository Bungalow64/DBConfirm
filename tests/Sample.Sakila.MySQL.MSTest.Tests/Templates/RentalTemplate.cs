using System;
using DBConfirm.Core.Data;
using DBConfirm.Core.Templates;
using DBConfirm.Core.Templates.Abstract;
using DBConfirm.Core.Templates.Placeholders;

namespace Sample.Sakila.MySQL.MSTest.Tests.Templates
{
    public class RentalTemplate : BaseIdentityTemplate<RentalTemplate>
    {
        public override string TableName => "`rental`";
        
        public override string IdentityColumnName => "rental_id";

        public override DataSetRow DefaultData => new DataSetRow
        {
            ["rental_date"] = DateTime.Parse("22-Oct-2020"),
            ["inventory_id"] = Placeholders.IsRequired(),
            ["customer_id"] = Placeholders.IsRequired(),
            ["staff_id"] = Placeholders.IsRequired()
        };

        public RentalTemplate WithRental_id(int value) => SetValue("rental_id", value);
        public RentalTemplate WithRental_date(DateTime value) => SetValue("rental_date", value);
        public RentalTemplate WithInventory_id(object value) => SetValue("inventory_id", value);
        public RentalTemplate WithInventory_id(IResolver resolver) => SetValue("inventory_id", resolver);
        public RentalTemplate WithCustomer_id(int value) => SetValue("customer_id", value);
        public RentalTemplate WithCustomer_id(IResolver resolver) => SetValue("customer_id", resolver);
        public RentalTemplate WithReturn_date(DateTime value) => SetValue("return_date", value);
        public RentalTemplate WithStaff_id(int value) => SetValue("staff_id", value);
        public RentalTemplate WithStaff_id(IResolver resolver) => SetValue("staff_id", resolver);
        public RentalTemplate WithLast_update(DateTime value) => SetValue("last_update", value);
    }
}
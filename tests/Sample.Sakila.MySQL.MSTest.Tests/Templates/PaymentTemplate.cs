using System;
using DBConfirm.Core.Data;
using DBConfirm.Core.Templates;
using DBConfirm.Core.Templates.Abstract;
using DBConfirm.Core.Templates.Placeholders;

namespace Sample.Sakila.MySQL.MSTest.Tests.Templates
{
    public class PaymentTemplate : BaseIdentityTemplate<PaymentTemplate>
    {
        public override string TableName => "`payment`";
        
        public override string IdentityColumnName => "payment_id";

        public override DataSetRow DefaultData => new DataSetRow
        {
            ["customer_id"] = Placeholders.IsRequired(),
            ["staff_id"] = Placeholders.IsRequired(),
            ["amount"] = 50.5,
            ["payment_date"] = DateTime.Parse("22-Oct-2020")
        };

        public PaymentTemplate WithPayment_id(int value) => SetValue("payment_id", value);
        public PaymentTemplate WithCustomer_id(int value) => SetValue("customer_id", value);
        public PaymentTemplate WithCustomer_id(IResolver resolver) => SetValue("customer_id", resolver);
        public PaymentTemplate WithStaff_id(int value) => SetValue("staff_id", value);
        public PaymentTemplate WithStaff_id(IResolver resolver) => SetValue("staff_id", resolver);
        public PaymentTemplate WithRental_id(int value) => SetValue("rental_id", value);
        public PaymentTemplate WithRental_id(IResolver resolver) => SetValue("rental_id", resolver);
        public PaymentTemplate WithAmount(decimal value) => SetValue("amount", value);
        public PaymentTemplate WithPayment_date(DateTime value) => SetValue("payment_date", value);
        public PaymentTemplate WithLast_update(DateTime value) => SetValue("last_update", value);
    }
}
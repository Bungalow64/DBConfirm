using System;
using DBConfirm.Core.Data;
using DBConfirm.Core.Templates;
using DBConfirm.Core.Templates.Abstract;
using DBConfirm.Core.Templates.Placeholders;

namespace Sample.Sakila.MySQL.MSTest.Tests.Templates
{
    public class StaffTemplate : BaseIdentityTemplate<StaffTemplate>
    {
        public override string TableName => "`staff`";
        
        public override string IdentityColumnName => "staff_id";

        public override DataSetRow DefaultData => new DataSetRow
        {
            ["first_name"] = "SampleFirst_name",
            ["last_name"] = "SampleLast_name",
            ["address_id"] = Placeholders.IsRequired(),
            ["store_id"] = Placeholders.IsRequired(),
            ["username"] = "SampleUsername"
        };

        public StaffTemplate WithStaff_id(int value) => SetValue("staff_id", value);
        public StaffTemplate WithFirst_name(string value) => SetValue("first_name", value);
        public StaffTemplate WithLast_name(string value) => SetValue("last_name", value);
        public StaffTemplate WithAddress_id(int value) => SetValue("address_id", value);
        public StaffTemplate WithAddress_id(IResolver resolver) => SetValue("address_id", resolver);
        public StaffTemplate WithPicture(object value) => SetValue("picture", value);
        public StaffTemplate WithEmail(string value) => SetValue("email", value);
        public StaffTemplate WithStore_id(int value) => SetValue("store_id", value);
        public StaffTemplate WithStore_id(IResolver resolver) => SetValue("store_id", resolver);
        public StaffTemplate WithActive(bool value) => SetValue("active", value);
        public StaffTemplate WithUsername(string value) => SetValue("username", value);
        public StaffTemplate WithPassword(string value) => SetValue("password", value);
        public StaffTemplate WithLast_update(DateTime value) => SetValue("last_update", value);
    }
}
using MySql.Data.Types;
using System;
using DBConfirm.Core.Data;
using DBConfirm.Core.Templates;
using DBConfirm.Core.Templates.Abstract;
using DBConfirm.Core.Templates.Placeholders;

namespace Sample.Sakila.MySQL.MSTest.Tests.Templates
{
    public class AddressTemplate : BaseIdentityTemplate<AddressTemplate>
    {
        public override string TableName => "`address`";
        
        public override string IdentityColumnName => "address_id";

        public override DataSetRow DefaultData => new DataSetRow
        {
            ["address"] = "SampleAddress",
            ["district"] = "SampleDistrict",
            ["city_id"] = Placeholders.IsRequired(),
            ["phone"] = "SamplePhone",
            ["location"] = new MySqlGeometry(1, 1)
        };

        public AddressTemplate WithAddress_id(int value) => SetValue("address_id", value);
        public AddressTemplate WithAddress(string value) => SetValue("address", value);
        public AddressTemplate WithAddress2(string value) => SetValue("address2", value);
        public AddressTemplate WithDistrict(string value) => SetValue("district", value);
        public AddressTemplate WithCity_id(int value) => SetValue("city_id", value);
        public AddressTemplate WithCity_id(IResolver resolver) => SetValue("city_id", resolver);
        public AddressTemplate WithPostal_code(string value) => SetValue("postal_code", value);
        public AddressTemplate WithPhone(string value) => SetValue("phone", value);
        public AddressTemplate WithLocation(MySqlGeometry value) => SetValue("location", value);
        public AddressTemplate WithLast_update(DateTime value) => SetValue("last_update", value);
    }
}
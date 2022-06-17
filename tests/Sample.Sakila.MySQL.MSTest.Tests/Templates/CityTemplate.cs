using System;
using DBConfirm.Core.Data;
using DBConfirm.Core.Templates;
using DBConfirm.Core.Templates.Abstract;
using DBConfirm.Core.Templates.Placeholders;

namespace Sample.Sakila.MySQL.MSTest.Tests.Templates
{
    public class CityTemplate : BaseIdentityTemplate<CityTemplate>
    {
        public override string TableName => "`city`";
        
        public override string IdentityColumnName => "city_id";

        public override DataSetRow DefaultData => new DataSetRow
        {
            ["city"] = "SampleCity",
            ["country_id"] = Placeholders.IsRequired()
        };

        public CityTemplate WithCity_id(int value) => SetValue("city_id", value);
        public CityTemplate WithCity(string value) => SetValue("city", value);
        public CityTemplate WithCountry_id(int value) => SetValue("country_id", value);
        public CityTemplate WithCountry_id(IResolver resolver) => SetValue("country_id", resolver);
        public CityTemplate WithLast_update(DateTime value) => SetValue("last_update", value);
    }
}
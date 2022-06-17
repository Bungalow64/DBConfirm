using System;
using DBConfirm.Core.Data;
using DBConfirm.Core.Templates;

namespace Sample.Sakila.MySQL.MSTest.Tests.Templates
{
    public class CountryTemplate : BaseIdentityTemplate<CountryTemplate>
    {
        public override string TableName => "`country`";
        
        public override string IdentityColumnName => "country_id";

        public override DataSetRow DefaultData => new DataSetRow
        {
            ["country"] = "SampleCountry"
        };

        public CountryTemplate WithCountry_id(int value) => SetValue("country_id", value);
        public CountryTemplate WithCountry(string value) => SetValue("country", value);
        public CountryTemplate WithLast_update(DateTime value) => SetValue("last_update", value);
    }
}
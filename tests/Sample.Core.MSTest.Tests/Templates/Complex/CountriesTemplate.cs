using SQLConfirm.Core.Data;
using SQLConfirm.Core.Templates;

namespace Sample.Core.MSTest.Tests.Templates.Complex
{
    public class CountriesTemplate: BaseSimpleTemplate<CountriesTemplate>
    {
        public override string TableName => "[dbo].[Countries]";
        
        public override DataSetRow DefaultData => new DataSetRow
        {
            ["CountryCode"] = "SampleCountryCode",
            ["CountryName"] = "SampleCountryName"
        };

        public CountriesTemplate WithCountryCode(string value) => SetValue("CountryCode", value);
        public CountriesTemplate WithCountryName(string value) => SetValue("CountryName", value);
    }
}
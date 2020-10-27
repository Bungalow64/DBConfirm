using SQLConfirm.Core.Data;
using SQLConfirm.Core.Templates;

namespace Sample.MSTest.Northwind.Tests.Templates
{
    public class RegionTemplate: BaseSimpleTemplate<RegionTemplate>
    {
        public override string TableName => "[dbo].[Region]";
        
        public override DataSetRow DefaultData => new DataSetRow
        {
            ["RegionID"] = 50,
            ["RegionDescription"] = "SampleRegionDescription"
        };

        public RegionTemplate WithRegionID(int value) => SetValue("RegionID", value);
        public RegionTemplate WithRegionDescription(string value) => SetValue("RegionDescription", value);
    }
}
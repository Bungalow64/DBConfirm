using DBConfirm.Core.Data;
using DBConfirm.Core.Templates;

namespace Sample.NUnit.Northwind.Tests.Templates;

public class RegionTemplate: BaseSimpleTemplate<RegionTemplate>
{
    public override string TableName => "[dbo].[Region]";
    
    public override DataSetRow DefaultData => new()
    {
        ["RegionID"] = 50,
        ["RegionDescription"] = "SampleRegionDescription"
    };

    public RegionTemplate WithRegionID(int value) => SetValue("RegionID", value);
    public RegionTemplate WithRegionDescription(string value) => SetValue("RegionDescription", value);
}
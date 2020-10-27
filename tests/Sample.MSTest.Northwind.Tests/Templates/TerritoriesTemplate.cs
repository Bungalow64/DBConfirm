using SQLConfirm.Core.Data;
using SQLConfirm.Core.Templates;
using SQLConfirm.Core.Templates.Placeholders;

namespace Sample.MSTest.Northwind.Tests.Templates
{
    public class TerritoriesTemplate: BaseSimpleTemplate<TerritoriesTemplate>
    {
        public override string TableName => "[dbo].[Territories]";
        
        public override DataSetRow DefaultData => new DataSetRow
        {
            ["TerritoryID"] = "SampleTerritoryID",
            ["TerritoryDescription"] = "SampleTerritoryDescription",
            ["RegionID"] = Placeholders.IsRequired()
        };

        public TerritoriesTemplate WithTerritoryID(string value) => SetValue("TerritoryID", value);
        public TerritoriesTemplate WithTerritoryDescription(string value) => SetValue("TerritoryDescription", value);
        public TerritoriesTemplate WithRegionID(int value) => SetValue("RegionID", value);
    }
}
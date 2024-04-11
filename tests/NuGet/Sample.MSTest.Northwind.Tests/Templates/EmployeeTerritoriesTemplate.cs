using DBConfirm.Core.Data;
using DBConfirm.Core.Templates;
using DBConfirm.Core.Templates.Abstract;
using DBConfirm.Core.Templates.Placeholders;

namespace Sample.MSTest.Northwind.Tests.Templates;

public class EmployeeTerritoriesTemplate: BaseSimpleTemplate<EmployeeTerritoriesTemplate>
{
    public override string TableName => "[dbo].[EmployeeTerritories]";
    
    public override DataSetRow DefaultData => new()
    {
        ["EmployeeID"] = Placeholders.IsRequired(),
        ["TerritoryID"] = Placeholders.IsRequired()
    };

    public EmployeeTerritoriesTemplate WithEmployeeID(int value) => SetValue("EmployeeID", value);
    public EmployeeTerritoriesTemplate WithEmployeeID(IResolver resolver) => SetValue("EmployeeID", resolver);
    public EmployeeTerritoriesTemplate WithTerritoryID(string value) => SetValue("TerritoryID", value);
}
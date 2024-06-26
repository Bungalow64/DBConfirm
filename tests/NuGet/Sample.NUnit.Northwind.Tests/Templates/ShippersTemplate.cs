using DBConfirm.Core.Data;
using DBConfirm.Core.Templates;

namespace Sample.NUnit.Northwind.Tests.Templates;

public class ShippersTemplate: BaseIdentityTemplate<ShippersTemplate>
{
    public override string TableName => "[dbo].[Shippers]";
    
    public override string IdentityColumnName => "ShipperID";

    public override DataSetRow DefaultData => new()
    {
        ["CompanyName"] = "SampleCompanyName"
    };

    public ShippersTemplate WithShipperID(int value) => SetValue("ShipperID", value);
    public ShippersTemplate WithCompanyName(string value) => SetValue("CompanyName", value);
    public ShippersTemplate WithPhone(string value) => SetValue("Phone", value);
}
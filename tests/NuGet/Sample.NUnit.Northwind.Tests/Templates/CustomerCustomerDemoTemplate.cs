using DBConfirm.Core.Data;
using DBConfirm.Core.Templates;
using DBConfirm.Core.Templates.Placeholders;

namespace Sample.NUnit.Northwind.Tests.Templates;

public class CustomerCustomerDemoTemplate: BaseSimpleTemplate<CustomerCustomerDemoTemplate>
{
    public override string TableName => "[dbo].[CustomerCustomerDemo]";
    
    public override DataSetRow DefaultData => new()
    {
        ["CustomerID"] = Placeholders.IsRequired(),
        ["CustomerTypeID"] = Placeholders.IsRequired()
    };

    public CustomerCustomerDemoTemplate WithCustomerID(string value) => SetValue("CustomerID", value);
    public CustomerCustomerDemoTemplate WithCustomerTypeID(string value) => SetValue("CustomerTypeID", value);
}
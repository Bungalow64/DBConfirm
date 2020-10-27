using SQLConfirm.Core.Data;
using SQLConfirm.Core.Templates;

namespace Sample.MSTest.Northwind.Tests.Templates
{
    public class CustomerDemographicsTemplate: BaseSimpleTemplate<CustomerDemographicsTemplate>
    {
        public override string TableName => "[dbo].[CustomerDemographics]";
        
        public override DataSetRow DefaultData => new DataSetRow
        {
            ["CustomerTypeID"] = "SampleCust"
        };

        public CustomerDemographicsTemplate WithCustomerTypeID(string value) => SetValue("CustomerTypeID", value);
        public CustomerDemographicsTemplate WithCustomerDesc(string value) => SetValue("CustomerDesc", value);
    }
}
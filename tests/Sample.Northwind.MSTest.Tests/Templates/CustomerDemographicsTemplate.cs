using DBConfirm.Core.Data;
using DBConfirm.Core.Templates;

namespace Sample.Northwind.MSTest.Tests.Templates
{
    public class CustomerDemographicsTemplate : BaseSimpleTemplate<CustomerDemographicsTemplate>
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
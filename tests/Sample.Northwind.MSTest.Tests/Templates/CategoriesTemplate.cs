using DBConfirm.Core.Data;
using DBConfirm.Core.Templates;

namespace Sample.Northwind.MSTest.Tests.Templates
{
    public class CategoriesTemplate : BaseIdentityTemplate<CategoriesTemplate>
    {
        public override string TableName => "[dbo].[Categories]";
        
        public override string IdentityColumnName => "CategoryID";

        public override DataSetRow DefaultData => new DataSetRow
        {
            ["CategoryName"] = "SampleCategoryN"
        };

        public CategoriesTemplate WithCategoryID(int value) => SetValue("CategoryID", value);
        public CategoriesTemplate WithCategoryName(string value) => SetValue("CategoryName", value);
        public CategoriesTemplate WithDescription(string value) => SetValue("Description", value);
        public CategoriesTemplate WithPicture(byte[] value) => SetValue("Picture", value);
    }
}
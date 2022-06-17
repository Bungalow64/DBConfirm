using System;
using DBConfirm.Core.Data;
using DBConfirm.Core.Templates;

namespace Sample.Sakila.MySQL.MSTest.Tests.Templates
{
    public class CategoryTemplate : BaseIdentityTemplate<CategoryTemplate>
    {
        public override string TableName => "`category`";
        
        public override string IdentityColumnName => "category_id";

        public override DataSetRow DefaultData => new DataSetRow
        {
            ["name"] = "SampleName"
        };

        public CategoryTemplate WithCategory_id(int value) => SetValue("category_id", value);
        public CategoryTemplate WithName(string value) => SetValue("name", value);
        public CategoryTemplate WithLast_update(DateTime value) => SetValue("last_update", value);
    }
}
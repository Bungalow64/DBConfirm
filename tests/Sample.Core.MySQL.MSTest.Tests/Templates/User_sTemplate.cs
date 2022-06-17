using DBConfirm.Core.Data;
using DBConfirm.Core.Templates;

namespace Sample.Core.MySQL.MSTest.Tests.Templates
{
    public class User_sTemplate : BaseIdentityTemplate<User_sTemplate>
    {
        public override string TableName => "`User's`";
        
        public override string IdentityColumnName => "Id";

        public override DataSetRow DefaultData => new DataSetRow
        {
            
        };

        public User_sTemplate WithId(int value) => SetValue("Id", value);
        public User_sTemplate WithName(string value) => SetValue("Name", value);
    }
}
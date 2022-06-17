using DBConfirm.Core.Data;
using DBConfirm.Core.Templates;

namespace Sample.Core.MySQL.MSTest.Tests.Templates
{
    public class IdentityOnlyTableTemplate : BaseIdentityTemplate<IdentityOnlyTableTemplate>
    {
        public override string TableName => "`IdentityOnlyTable`";
        
        public override string IdentityColumnName => "Id";

        public override DataSetRow DefaultData => new DataSetRow
        {
            
        };

        public IdentityOnlyTableTemplate WithId(int value) => SetValue("Id", value);
    }
}
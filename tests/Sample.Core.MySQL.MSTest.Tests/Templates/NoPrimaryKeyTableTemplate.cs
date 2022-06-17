using DBConfirm.Core.Data;
using DBConfirm.Core.Templates;

namespace Sample.Core.MySQL.MSTest.Tests.Templates
{
    public class NoPrimaryKeyTableTemplate : BaseSimpleTemplate<NoPrimaryKeyTableTemplate>
    {
        public override string TableName => "`NoPrimaryKeyTable`";
        
        public override DataSetRow DefaultData => new DataSetRow
        {
            
        };

        public NoPrimaryKeyTableTemplate WithId(int value) => SetValue("Id", value);
    }
}
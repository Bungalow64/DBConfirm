using DBConfirm.Core.Data;
using DBConfirm.Core.Templates;

namespace Sample.Core.MySQL.MSTest.Tests.Templates
{
    public class FXC15NX9HYBS0J8RHT6YHB9JJIPS2TWZQ2MA9C05I70WYG83LG877Q03X1XBGKLITemplate : BaseIdentityTemplate<FXC15NX9HYBS0J8RHT6YHB9JJIPS2TWZQ2MA9C05I70WYG83LG877Q03X1XBGKLITemplate>
    {
        public override string TableName => "`FXC15NX9HYBS0J8RHT6YHB9JJIPS2TWZQ2MA9C05I70WYG83LG877Q03X1XBGKLI`";
        
        public override string IdentityColumnName => "Id";

        public override DataSetRow DefaultData => new DataSetRow
        {
            
        };

        public FXC15NX9HYBS0J8RHT6YHB9JJIPS2TWZQ2MA9C05I70WYG83LG877Q03X1XBGKLITemplate WithId(int value) => SetValue("Id", value);
        public FXC15NX9HYBS0J8RHT6YHB9JJIPS2TWZQ2MA9C05I70WYG83LG877Q03X1XBGKLITemplate WithName(string value) => SetValue("Name", value);
    }
}
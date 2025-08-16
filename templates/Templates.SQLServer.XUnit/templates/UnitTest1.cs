using DBConfirm.Core.Data;
using DBConfirm.Core.DataResults;
using DBConfirm.Core.Parameters;
using DBConfirm.Packages.SQLServer.XUnit;
using XUnit;
using System.Threading.Tasks;

namespace Default.Templates.SQLServer.XUnit
{
    public class Tests : XUnitBase
    {
        [Fact]
        public async Task Test1()
        {
            Assert.Pass();
        }
    }
}
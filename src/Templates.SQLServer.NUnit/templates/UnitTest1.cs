using DBConfirm.Core.Data;
using DBConfirm.Core.DataResults;
using DBConfirm.Core.Parameters;
using DBConfirm.Packages.SQLServer.NUnit;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Default.Templates.SQLServer.NUnit
{
    [TestFixture]
    public class Tests : NUnitBase
    {
        [Test]
        public async Task Test1()
        {
            Assert.Pass();
        }
    }
}
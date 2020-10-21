using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLConfirm.Core.DataResults;
using System.Threading.Tasks;
using SQLConfirm.Core.Runners.Abstract;
using SQLConfirm.Frameworks.MSTest;

namespace Sample.Core.MSTest.Tests.Views
{
    [TestClass]
    public class AllUsersNewConnectionTests : TestBase
    {
        [TestMethod]
        public async Task AllUsersNewConnection_NoData_NothingReturned()
        {
            using (ITestRunner testRunner = await NewConnectionAsync("SERVER=(local);DATABASE=SampleDB;Integrated Security=true;Connection Timeout=30;"))
            {
                QueryResult results = await testRunner.ExecuteViewAsync("dbo.AllUsers");

                results
                    .AssertRowCount(0);

                Assert.AreEqual(0, await testRunner.CountRowsInViewAsync("dbo.AllUsers"));
            }
        }
    }
}

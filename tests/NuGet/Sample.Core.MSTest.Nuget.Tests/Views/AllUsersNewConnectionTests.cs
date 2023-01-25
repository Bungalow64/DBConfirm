using Microsoft.VisualStudio.TestTools.UnitTesting;
using DBConfirm.Core.DataResults;
using System.Threading.Tasks;
using DBConfirm.Core.Runners.Abstract;
using DBConfirm.Packages.SQLServer.MSTest;

namespace Sample.Core.MSTest.Nuget.Tests.Views
{
    [TestClass]
    public class AllUsersNewConnectionTests : MSTestBase
    {
        [TestMethod]
        public async Task AllUsersNewConnection_NoData_NothingReturned()
        {
            using (ITestRunner testRunner = await NewConnectionByConnectionStringAsync("SERVER=localhost,1401;DATABASE=SampleDB;User Id=sa;Password=123qwe123qwe!;Connection Timeout=30;"))
            {
                QueryResult results = await testRunner.ExecuteViewAsync("dbo.AllUsers");

                results
                    .AssertRowCount(0);

                Assert.AreEqual(0, await testRunner.CountRowsInViewAsync("dbo.AllUsers"));
            }
        }

        [TestMethod]
        public async Task AllUsersNewConnectionByName_NoData_NothingReturned()
        {
            using (ITestRunner testRunner = await NewConnectionByConnectionStringNameAsync("SampleDBConnection"))
            {
                QueryResult results = await testRunner.ExecuteViewAsync("dbo.AllUsers");

                results
                    .AssertRowCount(0);

                Assert.AreEqual(0, await testRunner.CountRowsInViewAsync("dbo.AllUsers"));
            }
        }
    }
}

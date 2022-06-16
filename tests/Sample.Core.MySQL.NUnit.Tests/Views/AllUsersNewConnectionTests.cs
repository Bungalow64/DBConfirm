using DBConfirm.Core.DataResults;
using DBConfirm.Core.Runners.Abstract;
using DBConfirm.Packages.MySQL.NUnit;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Sample.Core.MySQL.NUnit.Tests.Views
{
    [TestFixture]
    public class AllUsersNewConnectionTests : NUnitBase
    {
        [Test]
        public async Task AllUsersNewConnection_NoData_NothingReturned()
        {
            using (ITestRunner testRunner = await NewConnectionByConnectionStringAsync("SERVER=localhost;PORT=3306;DATABASE=SampleDB;User Id=root;Password=123qwe123qwe!;Connection Timeout=30;"))
            {
                QueryResult results = await testRunner.ExecuteViewAsync("AllUsers");

                results
                    .AssertRowCount(0);

                Assert.AreEqual(0, await testRunner.CountRowsInViewAsync("AllUsers"));
            }
        }

        [Test]
        public async Task AllUsersNewConnectionByName_NoData_NothingReturned()
        {
            using (ITestRunner testRunner = await NewConnectionByConnectionStringNameAsync("SampleDBConnection"))
            {
                QueryResult results = await testRunner.ExecuteViewAsync("AllUsers");

                results
                    .AssertRowCount(0);

                Assert.AreEqual(0, await testRunner.CountRowsInViewAsync("AllUsers"));
            }
        }
    }
}

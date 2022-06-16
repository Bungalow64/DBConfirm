using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sample.Core.MySQL.MSTest.Tests.Common;

namespace Sample.Core.MySQL.MSTest.Tests
{
    [TestClass]
    public class CorrectnessTests
    {
        [TestMethod]
        public void Correctness_VerifyConnectionString()
        {
            string connection = Initialisation.InitConfiguration().GetConnectionString("SampleDBConnection");
            Assert.AreEqual("SERVER=localhost;PORT=3306;DATABASE=SampleDB;User Id=root;Password=123qwe123qwe!;Connection Timeout=30;", connection);
        }
    }
}

using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Sample.Core.SQLServer.NUnit.Tests.Common;

namespace Sample.Core.SQLServer.NUnit.Tests
{
    [TestFixture]
    public class CorrectnessTests
    {
        [Test]
        public void Correctness_VerifyConnectionString()
        {
            string connection = Initialisation.InitConfiguration().GetConnectionString("DefaultConnectionString");
            Assert.AreEqual("SERVER=localhost,1401;DATABASE=SampleDB;User Id=sa;Password=123qwe123qwe!;Connection Timeout=30;", connection);
        }
    }
}

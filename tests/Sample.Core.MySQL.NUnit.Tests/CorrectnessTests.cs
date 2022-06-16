using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Sample.Core.MySQL.NUnit.Tests.Common;

namespace Sample.Core.MySQL.NUnit.Tests
{
    [TestFixture]
    public class CorrectnessTests
    {
        [Test]
        public void Correctness_VerifyConnectionString()
        {
            string connection = Initialisation.InitConfiguration().GetConnectionString("SampleDBConnection");
            Assert.AreEqual("SERVER=localhost;PORT=1507;DATABASE=SampleDB;User Id=root;Password=123qwe123qwe!;Connection Timeout=30;", connection);
        }
    }
}

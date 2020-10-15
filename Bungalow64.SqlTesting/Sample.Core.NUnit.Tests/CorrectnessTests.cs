using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Sample.Core.NUnit.Tests.Common;

namespace Sample.Core.NUnit.Tests
{
    [TestFixture]
    public class CorrectnessTests
    {
        [Test]
        public void Correctness_VerifyConnectionString()
        {
            string connection = Initialisation.InitConfiguration().GetConnectionString("TestDatabase");
            Assert.AreEqual("SERVER=(local);DATABASE=SampleDB;Integrated Security=true;Connection Timeout=30;", connection);
        }
    }
}

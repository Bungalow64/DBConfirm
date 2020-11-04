using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sample.Core.MSTest.Tests.Common;

namespace Sample.Core.MSTest.Tests
{
    [TestClass]
    public class CorrectnessTests
    {
        [TestMethod]
        public void Correctness_VerifyConnectionString()
        {
            string connection = Initialisation.InitConfiguration().GetConnectionString("SampleDBConnection");
            Assert.AreEqual("SERVER=(local);DATABASE=SampleDB;Integrated Security=true;Connection Timeout=30;", connection);
        }
    }
}

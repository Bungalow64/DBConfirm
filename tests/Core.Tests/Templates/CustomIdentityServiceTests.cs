using NUnit.Framework;
using DBConfirm.Core.Templates;

namespace Core.Tests.Templates
{
    [TestFixture]
    public class CustomIdentityServiceTests
    {
        [Test]
        public void CustomIdentityService_GetNextIdentity_ValueIncrements()
        {
            int value1 = CustomIdentityService.GenerateNextIdentity();
            int value2 = CustomIdentityService.GenerateNextIdentity();
            int value3 = CustomIdentityService.GenerateNextIdentity();

            Assert.AreNotEqual(value1, value2);
            Assert.AreNotEqual(value1, value3);
            Assert.AreNotEqual(value2, value3);
        }
    }
}

using Models;
using Models.Templates.Asbtract;
using System.Threading.Tasks;

namespace Sample.Core.MSTest.Tests.Templates.Complex
{
    public class UserWithAddressTemplate : IComplexTemplate
    {
        public UserTemplate User { get; set; }

        public UserAddressTemplate UserAddress { get; set; }

        public UserWithAddressTemplate()
        {
            User = new UserTemplate();
            UserAddress = new UserAddressTemplate();
        }

        public async Task InsertAsync(TestRunner testRunner)
        {
            await testRunner.InsertAsync(User);

            UserAddress["UserId"] = User.Identity;
            await testRunner.InsertAsync(UserAddress);
        }
    }
}

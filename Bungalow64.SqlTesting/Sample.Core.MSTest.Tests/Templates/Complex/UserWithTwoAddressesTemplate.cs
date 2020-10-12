using Models;
using Models.Templates.Asbtract;
using System.Threading.Tasks;

namespace Sample.Core.MSTest.Tests.Templates.Complex
{
    public class UserWithTwoAddressesTemplate : IComplexTemplate
    {
        public UserTemplate User { get; set; }

        public UserAddressTemplate UserAddress1 { get; set; }

        public UserAddressTemplate UserAddress2 { get; set; }

        public UserWithTwoAddressesTemplate()
        {
            User = new UserTemplate();
            UserAddress1 = new UserAddressTemplate();
            UserAddress2 = new UserAddressTemplate();
        }

        public async Task InsertAsync(TestRunner testRunner)
        {
            await testRunner.InsertAsync(User);

            UserAddress1["UserId"] = User.Identity;
            await testRunner.InsertAsync(UserAddress1);

            UserAddress2["UserId"] = User.Identity;
            await testRunner.InsertAsync(UserAddress2);
        }
    }
}

using Models;
using Models.Templates;
using System.Threading.Tasks;

namespace Sample.Core.MSTest.Tests.Templates.Complex
{
    public class UserWithTwoAddressesTemplate : BaseComplexTemplate
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

        public override async Task InsertAsync(TestRunner testRunner)
        {
            await testRunner.InsertTemplateAsync(User);

            UserAddress1["UserId"] = User.Identity;
            await testRunner.InsertTemplateAsync(UserAddress1);

            UserAddress2["UserId"] = User.Identity;
            await testRunner.InsertTemplateAsync(UserAddress2);
        }
    }
}

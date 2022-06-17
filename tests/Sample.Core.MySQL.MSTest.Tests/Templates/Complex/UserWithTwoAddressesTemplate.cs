using DBConfirm.Core.Runners.Abstract;
using DBConfirm.Core.Templates;
using System.Threading.Tasks;

namespace Sample.Core.MySQL.MSTest.Tests.Templates.Complex
{
    public class UserWithTwoAddressesTemplate : BaseComplexTemplate
    {
        public UsersTemplate User { get; set; }

        public UserAddressesTemplate UserAddress1 { get; set; }

        public UserAddressesTemplate UserAddress2 { get; set; }

        public UserWithTwoAddressesTemplate()
        {
            User = new UsersTemplate();
            UserAddress1 = new UserAddressesTemplate();
            UserAddress2 = new UserAddressesTemplate();
        }

        public override async Task InsertAsync(ITestRunner testRunner)
        {
            await testRunner.InsertTemplateAsync(User);

            UserAddress1["UserId"] = User.Identity;
            await testRunner.InsertTemplateAsync(UserAddress1);

            UserAddress2["UserId"] = User.Identity;
            await testRunner.InsertTemplateAsync(UserAddress2);
        }
    }
}

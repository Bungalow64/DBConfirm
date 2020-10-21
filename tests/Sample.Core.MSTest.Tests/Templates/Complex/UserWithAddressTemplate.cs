using SQLConfirm.Core.Runners.Abstract;
using SQLConfirm.Core.Templates;
using System.Threading.Tasks;

namespace Sample.Core.MSTest.Tests.Templates.Complex
{
    public class UserWithAddressTemplate : BaseComplexTemplate
    {
        public UserTemplate User { get; set; }

        public UserAddressTemplate UserAddress { get; set; }

        public UserWithAddressTemplate()
        {
            User = new UserTemplate();
            UserAddress = new UserAddressTemplate();
        }

        public override async Task InsertAsync(ITestRunner testRunner)
        {
            await testRunner.InsertTemplateAsync(User);

            UserAddress["UserId"] = User.Identity;
            await testRunner.InsertTemplateAsync(UserAddress);
        }
    }
}

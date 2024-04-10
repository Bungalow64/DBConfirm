using DBConfirm.Core.Runners.Abstract;
using DBConfirm.Core.Templates;
using System.Threading.Tasks;

namespace Sample.Core.NUnit.Nuget.Tests.Templates.Complex;

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

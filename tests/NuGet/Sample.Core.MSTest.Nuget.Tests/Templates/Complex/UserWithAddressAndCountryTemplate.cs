using DBConfirm.Core.Runners.Abstract;
using DBConfirm.Core.Templates;
using System.Threading.Tasks;

namespace Sample.Core.MSTest.Nuget.Tests.Templates.Complex;

public class UserWithAddressAndCountryTemplate : BaseComplexTemplate
{
    public UserTemplate User { get; set; } = [];

    public UserAddressTemplate UserAddress { get; set; } = [];

    public CountriesTemplate Country { get; set; } = [];

    public override async Task InsertAsync(ITestRunner testRunner)
    {
        await testRunner.InsertTemplateAsync(User);

        if (!Country.IsInserted)
        {
            await testRunner.InsertTemplateAsync(Country);
        }

        UserAddress["UserId"] = User.Identity;
        UserAddress["CountryCode"] = Country["CountryCode"];
        await testRunner.InsertTemplateAsync(UserAddress);
    }
}

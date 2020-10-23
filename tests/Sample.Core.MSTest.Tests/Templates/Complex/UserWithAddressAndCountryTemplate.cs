using SQLConfirm.Core.Runners.Abstract;
using SQLConfirm.Core.Templates;
using System.Threading.Tasks;

namespace Sample.Core.MSTest.Tests.Templates.Complex
{
    public class UserWithAddressAndCountryTemplate : BaseComplexTemplate
    {
        public UserTemplate User { get; set; } = new UserTemplate();

        public UserAddressTemplate UserAddress { get; set; } = new UserAddressTemplate();

        public CountriesTemplate Country { get; set; } = new CountriesTemplate();

        public override async Task InsertAsync(ITestRunner testRunner)
        {
            await testRunner.InsertTemplateAsync(User);

            if (!Country.IsInserted)
            {
                await testRunner.InsertTemplateAsync(Country);
            }

            UserAddress["UserId"] = User.Identity;
            UserAddress["CountryCode"] = Country.MergedData["CountryCode"];
            await testRunner.InsertTemplateAsync(UserAddress);
        }
    }
}

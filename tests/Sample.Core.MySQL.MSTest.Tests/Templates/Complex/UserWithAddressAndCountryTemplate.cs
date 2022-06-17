using DBConfirm.Core.Runners.Abstract;
using DBConfirm.Core.Templates;
using System.Threading.Tasks;

namespace Sample.Core.MySQL.MSTest.Tests.Templates.Complex
{
    public class UserWithAddressAndCountryTemplate : BaseComplexTemplate
    {
        public UsersTemplate User { get; set; } = new UsersTemplate();

        public UserAddressesTemplate UserAddress { get; set; } = new UserAddressesTemplate();

        public CountriesTemplate Country { get; set; } = new CountriesTemplate();

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
}

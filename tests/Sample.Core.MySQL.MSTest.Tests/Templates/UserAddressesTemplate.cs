using DBConfirm.Core.Data;
using DBConfirm.Core.Templates;
using DBConfirm.Core.Templates.Abstract;
using DBConfirm.Core.Templates.Placeholders;

namespace Sample.Core.MySQL.MSTest.Tests.Templates
{
    public class UserAddressesTemplate : BaseIdentityTemplate<UserAddressesTemplate>
    {
        public override string TableName => "`UserAddresses`";
        
        public override string IdentityColumnName => "Id";

        public override DataSetRow DefaultData => new DataSetRow
        {
            ["UserId"] = Placeholders.IsRequired(),
            ["Address1"] = "31 High Street",
            ["Postcode"] = "HD6 3UB"
        };

        public UserAddressesTemplate WithId(int value) => SetValue("Id", value);
        public UserAddressesTemplate WithUserId(int value) => SetValue("UserId", value);
        public UserAddressesTemplate WithUserId(IResolver resolver) => SetValue("UserId", resolver);
        public UserAddressesTemplate WithAddress1(string value) => SetValue("Address1", value);
        public UserAddressesTemplate WithPostcode(string value) => SetValue("Postcode", value);
        public UserAddressesTemplate WithOther(string value) => SetValue("Other", value);
        public UserAddressesTemplate WithCountryCode(string value) => SetValue("CountryCode", value);
    }
}
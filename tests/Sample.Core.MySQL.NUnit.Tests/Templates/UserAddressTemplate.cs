using DBConfirm.Core.Data;
using DBConfirm.Core.Templates;
using DBConfirm.Core.Templates.Placeholders;

namespace Sample.Core.MySQL.NUnit.Tests.Templates
{
    public class UserAddressTemplate : BaseIdentityTemplate<UserAddressTemplate>
    {
        public override string TableName => "`UserAddresses`";

        public override string IdentityColumnName => "Id";

        public override DataSetRow DefaultData => new DataSetRow
        {
            { "UserId", Placeholders.IsRequired() },
            { "Address1", "31 High Street" },
            { "Postcode", "HD6 3UB" },
            { "Other", null }
        };

        public UserAddressTemplate() { }

        public UserAddressTemplate(UserTemplate user)
        {
            this["UserId"] = user.IdentityResolver;
        }

        public UserAddressTemplate WithId(int value) => SetValue(IdentityColumnName, value);

        public UserAddressTemplate WithUserId(int value) => SetValue("UserId", value);
        public UserAddressTemplate WithAddress1(string value) => SetValue("Address1", value);
        public UserAddressTemplate WithPostcode(string value) => SetValue("Postcode", value);
        public UserAddressTemplate WithOther(string value) => SetValue("Other", value);
    }
}

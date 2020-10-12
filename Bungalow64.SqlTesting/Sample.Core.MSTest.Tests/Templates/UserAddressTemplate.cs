using Models;
using Models.Templates;
using Models.Templates.Placeholders;

namespace Sample.Core.MSTest.Tests.Templates
{
    public class UserAddressTemplate : BaseIdentityTemplate
    {
        public override string TableName => "dbo.UserAddresses";

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
    }
}

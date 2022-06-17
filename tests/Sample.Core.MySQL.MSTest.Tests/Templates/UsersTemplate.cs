using DBConfirm.Core.Data;
using DBConfirm.Core.Templates;
using System;

namespace Sample.Core.MySQL.MSTest.Tests.Templates
{
    public class UsersTemplate : BaseIdentityTemplate<UsersTemplate>
    {
        public override string TableName => "`Users`";

        public override string IdentityColumnName => "Id";

        public override DataSetRow DefaultData => new DataSetRow
        {
            { "FirstName", "Jamie" },
            { "LastName", "Burns" },
            { "EmailAddress", "jamie@bungalow64.co.uk" },
            { "StartDate", DateTime.Parse("01-Mar-2020") },
            { "NumberOfHats", 14 },
            { "Cost", 15.87 },
            { "CreatedDate", DateTime.UtcNow }
        };

        public UsersTemplate WithId(int value) => SetValue(IdentityColumnName, value);

        public UsersTemplate WithFirstName(string value) => SetValue("FirstName", value);
        public UsersTemplate WithLastName(string value) => SetValue("LastName", value);
        public UsersTemplate WithEmailAddress(string value) => SetValue("EmailAddress", value);
        public UsersTemplate WithStartDate(DateTime value) => SetValue("StartDate", value);
        public UsersTemplate WithNumberOfHats(int value) => SetValue("NumberOfHats", value);
        public UsersTemplate WithCost(decimal value) => SetValue("Cost", value);
        public UsersTemplate WithCreatedDate(DateTime value) => SetValue("CreatedDate", value);
    }
}

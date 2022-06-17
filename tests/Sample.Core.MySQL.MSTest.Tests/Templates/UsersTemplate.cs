using System;
using DBConfirm.Core.Data;
using DBConfirm.Core.Templates;

namespace Sample.Core.MySQL.MSTest.Tests.Templates
{
    public class UsersTemplate : BaseIdentityTemplate<UsersTemplate>
    {
        public override string TableName => "`Users`";
        
        public override string IdentityColumnName => "Id";

        public override DataSetRow DefaultData => new DataSetRow
        {
            ["FirstName"] = "Jamie",
            ["LastName"] = "Burns",
            ["EmailAddress"] = "jamie@bungalow64.co.uk",
            ["CreatedDate"] = DateTime.UtcNow,
            ["StartDate"] = DateTime.Parse("01-Mar-2020"),
            ["NumberOfHats"] = 14,
            ["Cost"] = 15.87
        };

        public UsersTemplate WithId(int value) => SetValue("Id", value);
        public UsersTemplate WithFirstName(string value) => SetValue("FirstName", value);
        public UsersTemplate WithLastName(string value) => SetValue("LastName", value);
        public UsersTemplate WithEmailAddress(string value) => SetValue("EmailAddress", value);
        public UsersTemplate WithCreatedDate(DateTime value) => SetValue("CreatedDate", value);
        public UsersTemplate WithStartDate(DateTime value) => SetValue("StartDate", value);
        public UsersTemplate WithIsActive(bool value) => SetValue("IsActive", value);
        public UsersTemplate WithNumberOfHats(long value) => SetValue("NumberOfHats", value);
        public UsersTemplate WithHatType(string value) => SetValue("HatType", value);
        public UsersTemplate WithCost(decimal value) => SetValue("Cost", value);
    }
}
using DBConfirm.Core.Data;
using DBConfirm.Core.Templates;
using System;

namespace Sample.Core.NUnit.Nuget.Tests.Templates;

public class UserTemplate : BaseIdentityTemplate<UserTemplate>
{
    public override string TableName => "dbo.Users";

    public override string IdentityColumnName => "Id";

    public override DataSetRow DefaultData => new()
    {
        { "FirstName", "Jamie" },
        { "LastName", "Burns" },
        { "EmailAddress", "jamie@bungalow64.co.uk" },
        { "StartDate", DateTime.Parse("01-Mar-2020") },
        { "NumberOfHats", 14 },
        { "Cost", 15.87 },
        { "CreatedDate", DateTime.UtcNow }
    };

    public UserTemplate WithId(int value) => SetValue(IdentityColumnName, value);

    public UserTemplate WithFirstName(string value) => SetValue("FirstName", value);
    public UserTemplate WithLastName(string value) => SetValue("LastName", value);
    public UserTemplate WithEmailAddress(string value) => SetValue("EmailAddress", value);
    public UserTemplate WithStartDate(DateTime value) => SetValue("StartDate", value);
    public UserTemplate WithNumberOfHats(int value) => SetValue("NumberOfHats", value);
    public UserTemplate WithCost(decimal value) => SetValue("Cost", value);
    public UserTemplate WithCreatedDate(DateTime value) => SetValue("CreatedDate", value);
}

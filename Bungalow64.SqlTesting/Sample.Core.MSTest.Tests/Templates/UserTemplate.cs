using Models;
using Models.Templates;
using System;

namespace Sample.Core.MSTest.Tests.Templates
{
    public class UserTemplate : BaseIdentityTemplate
    {
        public override string TableName => "dbo.Users";

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
    }
}

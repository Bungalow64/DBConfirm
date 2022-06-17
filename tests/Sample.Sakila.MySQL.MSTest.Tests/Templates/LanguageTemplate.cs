using System;
using DBConfirm.Core.Data;
using DBConfirm.Core.Templates;

namespace Sample.Sakila.MySQL.MSTest.Tests.Templates
{
    public class LanguageTemplate : BaseIdentityTemplate<LanguageTemplate>
    {
        public override string TableName => "`language`";
        
        public override string IdentityColumnName => "language_id";

        public override DataSetRow DefaultData => new DataSetRow
        {
            ["name"] = "SampleName"
        };

        public LanguageTemplate WithLanguage_id(int value) => SetValue("language_id", value);
        public LanguageTemplate WithName(string value) => SetValue("name", value);
        public LanguageTemplate WithLast_update(DateTime value) => SetValue("last_update", value);
    }
}
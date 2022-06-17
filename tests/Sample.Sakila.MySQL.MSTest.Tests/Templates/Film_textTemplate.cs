using DBConfirm.Core.Data;
using DBConfirm.Core.Templates;

namespace Sample.Sakila.MySQL.MSTest.Tests.Templates
{
    public class Film_textTemplate : BaseSimpleTemplate<Film_textTemplate>
    {
        public override string TableName => "`film_text`";
        
        public override DataSetRow DefaultData => new DataSetRow
        {
            ["film_id"] = 50,
            ["title"] = "SampleTitle"
        };

        public Film_textTemplate WithFilm_id(int value) => SetValue("film_id", value);
        public Film_textTemplate WithTitle(string value) => SetValue("title", value);
        public Film_textTemplate WithDescription(string value) => SetValue("description", value);
    }
}
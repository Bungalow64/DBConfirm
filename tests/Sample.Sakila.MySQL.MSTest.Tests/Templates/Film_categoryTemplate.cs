using System;
using DBConfirm.Core.Data;
using DBConfirm.Core.Templates;
using DBConfirm.Core.Templates.Abstract;
using DBConfirm.Core.Templates.Placeholders;

namespace Sample.Sakila.MySQL.MSTest.Tests.Templates
{
    public class Film_categoryTemplate : BaseSimpleTemplate<Film_categoryTemplate>
    {
        public override string TableName => "`film_category`";
        
        public override DataSetRow DefaultData => new DataSetRow
        {
            ["film_id"] = Placeholders.IsRequired(),
            ["category_id"] = Placeholders.IsRequired()
        };

        public Film_categoryTemplate WithFilm_id(int value) => SetValue("film_id", value);
        public Film_categoryTemplate WithFilm_id(IResolver resolver) => SetValue("film_id", resolver);
        public Film_categoryTemplate WithCategory_id(int value) => SetValue("category_id", value);
        public Film_categoryTemplate WithCategory_id(IResolver resolver) => SetValue("category_id", resolver);
        public Film_categoryTemplate WithLast_update(DateTime value) => SetValue("last_update", value);
    }
}
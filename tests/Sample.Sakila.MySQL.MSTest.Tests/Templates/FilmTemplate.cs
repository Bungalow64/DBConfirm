using System;
using DBConfirm.Core.Data;
using DBConfirm.Core.Templates;
using DBConfirm.Core.Templates.Abstract;
using DBConfirm.Core.Templates.Placeholders;

namespace Sample.Sakila.MySQL.MSTest.Tests.Templates
{
    public class FilmTemplate : BaseIdentityTemplate<FilmTemplate>
    {
        public override string TableName => "`film`";
        
        public override string IdentityColumnName => "film_id";

        public override DataSetRow DefaultData => new DataSetRow
        {
            ["title"] = "SampleTitle",
            ["language_id"] = Placeholders.IsRequired()
        };

        public FilmTemplate WithFilm_id(int value) => SetValue("film_id", value);
        public FilmTemplate WithTitle(string value) => SetValue("title", value);
        public FilmTemplate WithDescription(string value) => SetValue("description", value);
        public FilmTemplate WithRelease_year(object value) => SetValue("release_year", value);
        public FilmTemplate WithLanguage_id(int value) => SetValue("language_id", value);
        public FilmTemplate WithLanguage_id(IResolver resolver) => SetValue("language_id", resolver);
        public FilmTemplate WithOriginal_language_id(int value) => SetValue("original_language_id", value);
        public FilmTemplate WithOriginal_language_id(IResolver resolver) => SetValue("original_language_id", resolver);
        public FilmTemplate WithRental_duration(int value) => SetValue("rental_duration", value);
        public FilmTemplate WithRental_rate(decimal value) => SetValue("rental_rate", value);
        public FilmTemplate WithLength(int value) => SetValue("length", value);
        public FilmTemplate WithReplacement_cost(decimal value) => SetValue("replacement_cost", value);
        public FilmTemplate WithRating(object value) => SetValue("rating", value);
        public FilmTemplate WithSpecial_features(object value) => SetValue("special_features", value);
        public FilmTemplate WithLast_update(DateTime value) => SetValue("last_update", value);
    }
}
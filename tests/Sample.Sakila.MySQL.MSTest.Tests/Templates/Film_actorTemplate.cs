using System;
using DBConfirm.Core.Data;
using DBConfirm.Core.Templates;
using DBConfirm.Core.Templates.Abstract;
using DBConfirm.Core.Templates.Placeholders;

namespace Sample.Sakila.MySQL.MSTest.Tests.Templates
{
    public class Film_actorTemplate : BaseSimpleTemplate<Film_actorTemplate>
    {
        public override string TableName => "`film_actor`";
        
        public override DataSetRow DefaultData => new DataSetRow
        {
            ["actor_id"] = Placeholders.IsRequired(),
            ["film_id"] = Placeholders.IsRequired()
        };

        public Film_actorTemplate WithActor_id(int value) => SetValue("actor_id", value);
        public Film_actorTemplate WithActor_id(IResolver resolver) => SetValue("actor_id", resolver);
        public Film_actorTemplate WithFilm_id(int value) => SetValue("film_id", value);
        public Film_actorTemplate WithFilm_id(IResolver resolver) => SetValue("film_id", resolver);
        public Film_actorTemplate WithLast_update(DateTime value) => SetValue("last_update", value);
    }
}
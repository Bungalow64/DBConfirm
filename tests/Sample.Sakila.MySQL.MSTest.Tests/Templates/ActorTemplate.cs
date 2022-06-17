using System;
using DBConfirm.Core.Data;
using DBConfirm.Core.Templates;

namespace Sample.Sakila.MySQL.MSTest.Tests.Templates
{
    public class ActorTemplate : BaseIdentityTemplate<ActorTemplate>
    {
        public override string TableName => "`actor`";
        
        public override string IdentityColumnName => "actor_id";

        public override DataSetRow DefaultData => new DataSetRow
        {
            ["first_name"] = "SampleFirst_name",
            ["last_name"] = "SampleLast_name"
        };

        public ActorTemplate WithActor_id(int value) => SetValue("actor_id", value);
        public ActorTemplate WithFirst_name(string value) => SetValue("first_name", value);
        public ActorTemplate WithLast_name(string value) => SetValue("last_name", value);
        public ActorTemplate WithLast_update(DateTime value) => SetValue("last_update", value);
    }
}
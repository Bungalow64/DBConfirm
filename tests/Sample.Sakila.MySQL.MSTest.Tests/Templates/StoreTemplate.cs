using System;
using DBConfirm.Core.Data;
using DBConfirm.Core.Templates;
using DBConfirm.Core.Templates.Abstract;
using DBConfirm.Core.Templates.Placeholders;

namespace Sample.Sakila.MySQL.MSTest.Tests.Templates
{
    public class StoreTemplate : BaseIdentityTemplate<StoreTemplate>
    {
        public override string TableName => "`store`";
        
        public override string IdentityColumnName => "store_id";

        public override DataSetRow DefaultData => new DataSetRow
        {
            ["manager_staff_id"] = Placeholders.IsRequired(),
            ["address_id"] = Placeholders.IsRequired()
        };

        public StoreTemplate WithStore_id(int value) => SetValue("store_id", value);
        public StoreTemplate WithManager_staff_id(int value) => SetValue("manager_staff_id", value);
        public StoreTemplate WithManager_staff_id(IResolver resolver) => SetValue("manager_staff_id", resolver);
        public StoreTemplate WithAddress_id(int value) => SetValue("address_id", value);
        public StoreTemplate WithAddress_id(IResolver resolver) => SetValue("address_id", resolver);
        public StoreTemplate WithLast_update(DateTime value) => SetValue("last_update", value);
    }
}
using System;
using DBConfirm.Core.Data;
using DBConfirm.Core.Templates;
using DBConfirm.Core.Templates.Abstract;
using DBConfirm.Core.Templates.Placeholders;

namespace Sample.Sakila.MySQL.MSTest.Tests.Templates
{
    public class InventoryTemplate : BaseIdentityTemplate<InventoryTemplate>
    {
        public override string TableName => "`inventory`";
        
        public override string IdentityColumnName => "inventory_id";

        public override DataSetRow DefaultData => new DataSetRow
        {
            ["film_id"] = Placeholders.IsRequired(),
            ["store_id"] = Placeholders.IsRequired()
        };

        public InventoryTemplate WithInventory_id(object value) => SetValue("inventory_id", value);
        public InventoryTemplate WithFilm_id(int value) => SetValue("film_id", value);
        public InventoryTemplate WithFilm_id(IResolver resolver) => SetValue("film_id", resolver);
        public InventoryTemplate WithStore_id(int value) => SetValue("store_id", value);
        public InventoryTemplate WithStore_id(IResolver resolver) => SetValue("store_id", resolver);
        public InventoryTemplate WithLast_update(DateTime value) => SetValue("last_update", value);
    }
}
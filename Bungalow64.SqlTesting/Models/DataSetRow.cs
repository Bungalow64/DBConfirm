using Microsoft.Data.SqlClient;
using Models.Templates;
using Models.Templates.Asbtract;
using Models.Templates.Placeholders;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Models
{
    public class DataSetRow : Dictionary<string, object>
    {
        // TODO: Consider moving Dictionary into a property, instead of an inherited type.  Might make Intellisense clearer, and can limit what can be done with it (if that's wanted?)
        public new object this[string key]
        {
            get
            {
                if (ContainsKey(key))
                {
                    return base[key];
                }
                throw new KeyNotFoundException($"{key} was not found in the data set");
            }
            set
            {
                base[key] = value;
            }
        }

        public DataSetRow() { }

        public DataSetRow(Dictionary<string, object> data)
            :base(data)
        {

        }

        public override string ToString()
        {
            return Environment.NewLine + string.Join(Environment.NewLine, this);
        }

        public DataSetRow Merge(DataSetRow dictionary)
        {
            DataSetRow merged = new DataSetRow();
            this.ToList().ForEach(p => merged.Add(p.Key, p.Value));
            dictionary.ToList().ForEach(p => merged[p.Key] = p.Value);
            return merged;
        }
    }
}

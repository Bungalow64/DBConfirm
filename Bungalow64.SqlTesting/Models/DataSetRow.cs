using System;
using System.Collections.Generic;
using System.Linq;

namespace Models
{
    public class DataSetRow : Dictionary<string, object>
    {
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
            :base(data ?? new Dictionary<string, object>())
        {

        }

        public override string ToString()
        {
            if (Count == 0)
            {
                return string.Empty;
            }
            return Environment.NewLine + string.Join(Environment.NewLine, this);
        }

        public DataSetRow Merge(DataSetRow dictionary)
        {
            DataSetRow merged = new DataSetRow();
            this.ToList().ForEach(p => merged.Add(p.Key, p.Value));
            dictionary?.ToList().ForEach(p => merged[p.Key] = p.Value);
            return merged;
        }
    }
}

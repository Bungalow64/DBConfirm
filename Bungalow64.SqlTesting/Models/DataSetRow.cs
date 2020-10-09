using System;
using System.Collections.Generic;

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
            :base(data)
        {

        }

        public override string ToString()
        {
            return Environment.NewLine + string.Join(Environment.NewLine, this);
        }
    }
}

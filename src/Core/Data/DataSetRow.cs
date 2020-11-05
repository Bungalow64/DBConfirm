using System;
using System.Collections.Generic;
using System.Linq;

namespace DBConfirm.Core.Data
{
    /// <summary>
    /// Defines the data for a single row, in a single table
    /// </summary>
    public class DataSetRow : Dictionary<string, object>
    {
        /// <summary>
        /// Gets or sets the value for a specific column
        /// </summary>
        /// <param name="columnName">The name of the column</param>
        /// <returns>Returns the data stored for the column, or an exception if the column does not exist</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the column is not found in the data set</exception>
        public new object this[string columnName]
        {
            get
            {
                if (ContainsKey(columnName))
                {
                    return base[columnName];
                }
                throw new KeyNotFoundException($"{columnName} was not found in the data set");
            }
            set
            {
                base[columnName] = value;
            }
        }

        /// <summary>
        /// Default constructor, instantiating with an empty data set
        /// </summary>
        public DataSetRow() { }

        /// <summary>
        /// Constructor, instantiating with an existing dictionary of values
        /// </summary>
        /// <param name="data">The data used to instantiate the row.  The Key relates to the column name, and the Value relates to the column value</param>
        public DataSetRow(Dictionary<string, object> data)
            : base(data ?? new Dictionary<string, object>())
        {

        }

        /// <inheritdoc/>
        public override string ToString()
        {
            if (Count == 0)
            {
                return string.Empty;
            }
            return Environment.NewLine + string.Join(Environment.NewLine, this);
        }

        /// <summary>
        /// Merges the existing row data with a new data set, returning the resulting merge.  The two existing data sets are not changed by this merge.
        /// </summary>
        /// <param name="dictionary">The new data set to merge with</param>
        /// <returns>Returns a single data set, representing the merge of the two data sets</returns>
        public DataSetRow Merge(DataSetRow dictionary)
        {
            DataSetRow merged = Copy();
            dictionary?.ToList().ForEach(p => merged[p.Key] = p.Value);
            return merged;
        }

        /// <summary>
        /// Applies all values within the supplied <see cref="DataSetRow"/> to the current <see cref="DataSetRow"/>, overwriting all keys
        /// </summary>
        /// <param name="dictionary">The new data set to apply from</param>
        public void Apply(DataSetRow dictionary)
        {
            dictionary?.ToList().ForEach(p => this[p.Key] = p.Value);
        }

        /// <summary>
        /// Copies the <see cref="DataSetRow"/> to a new instance.  Uses a shallow copy.
        /// </summary>
        /// <returns>Returns the copied <see cref="DataSetRow"/></returns>
        public DataSetRow Copy()
        {
            DataSetRow copy = new DataSetRow();
            this.ToList().ForEach(p => copy.Add(p.Key, p.Value));
            return copy;
        }
    }
}

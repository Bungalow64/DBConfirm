using DBConfirm.Core.Data;
using DBConfirm.Core.Runners.Abstract;
using DBConfirm.Core.Templates.Abstract;
using System.Threading.Tasks;

namespace DBConfirm.Core.Templates
{
    /// <summary>
    /// The abstract template class used as the base for simple templates.  Simple templates are used to set up data in a single table
    /// </summary>
    /// <typeparam name="T">The type of the current template object</typeparam>
    public abstract class BaseSimpleTemplate<T> : DataSetRow, ISimpleTemplate
        where T : BaseSimpleTemplate<T>
    {
        /// <inheritdoc/>
        public abstract string TableName { get; }

        /// <inheritdoc/>
        public DataSetRow CustomData => this;

        /// <inheritdoc/>
        public abstract DataSetRow DefaultData { get; }

        /// <inheritdoc/>
        public bool IsInserted { get; private set; }

        /// <inheritdoc/>
        public DataSetRow MergedData => DefaultData.Merge(this);

        /// <summary>
        /// Constructor, setting no custom data
        /// </summary>
        protected BaseSimpleTemplate() : base() { }

        /// <summary>
        /// Constructor, with custom data
        /// </summary>
        protected BaseSimpleTemplate(DataSetRow data) : base(data) { }

        /// <inheritdoc/>
        public void RecordInsertion() => IsInserted = true;

        /// <summary>
        /// Sets a value in the custom data set
        /// </summary>
        /// <param name="columnName">The name of the column</param>
        /// <param name="value">The value to set</param>
        /// <returns>Returns the current <typeparamref name="T"/> object</returns>
        public T SetValue(string columnName, object value)
        {
            this[columnName] = value;
            return (T)this;
        }

        /// <inheritdoc/>
        public Task InsertAsync(ITestRunner testRunner) => testRunner.InsertDataAsync(TableName, DefaultData, CustomData);
    }
}

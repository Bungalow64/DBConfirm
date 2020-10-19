using Models.Templates.Asbtract;
using System.Threading.Tasks;

namespace Models.Templates
{
    public abstract class BaseSimpleTemplate<T> : DataSetRow, ISimpleTemplate
        where T : BaseSimpleTemplate<T>
    {
        public abstract string TableName { get; }

        public DataSetRow CustomData => this;

        public abstract DataSetRow DefaultData { get; }

        public bool IsInserted { get; private set; }

        public DataSetRow MergedData => DefaultData.Merge(this);

        protected BaseSimpleTemplate() : base() { }

        protected BaseSimpleTemplate(DataSetRow data) : base(data) { }

        public void RecordInsertion() => IsInserted = true;

        public T SetValue(string key, object value)
        {
            this[key] = value;
            return (T)this;
        }

        public Task InsertAsync(TestRunner testRunner) => testRunner.InsertDataAsync(TableName, DefaultData, CustomData);
    }
}

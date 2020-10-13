using Models.Templates.Asbtract;

namespace Models.Templates
{
    public abstract class BaseTemplate : DataSetRow, ITemplate
    {
        public abstract string TableName { get; }

        public DataSetRow CustomData => this;

        public abstract DataSetRow DefaultData { get; }

        public DataSetRow MergedData => DefaultData.Merge(this);

        protected BaseTemplate() : base() { }

        protected BaseTemplate(DataSetRow data) : base(data) { }
    }
}

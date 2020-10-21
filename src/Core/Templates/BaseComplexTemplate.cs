using SQLConfirm.Core.Runners.Abstract;
using SQLConfirm.Core.Templates.Abstract;
using System.Threading.Tasks;

namespace SQLConfirm.Core.Templates
{
    /// <summary>
    /// The abstract template class used as the base for complex templates.  Complex templates are used to build templates that insert data into multiple tables, including setting foreign keys
    /// </summary>
    public abstract class BaseComplexTemplate : ITemplate
    {
        /// <inheritdoc/>
        public bool IsInserted { get; private set; }

        /// <inheritdoc/>
        public void RecordInsertion() => IsInserted = true;

        /// <inheritdoc/>
        public abstract Task InsertAsync(ITestRunner testRunner);
    }
}

using Models.Abstract;
using Models.Templates.Asbtract;
using System.Threading.Tasks;

namespace Models.Templates
{
    public abstract class BaseComplexTemplate : ITemplate
    {
        public bool IsInserted { get; private set; }

        public void RecordInsertion() => IsInserted = true;

        public abstract Task InsertAsync(ITestRunner testRunner);
    }
}

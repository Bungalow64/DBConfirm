using System.Threading.Tasks;

namespace Models.Templates.Asbtract
{
    public interface ITemplate
    {
        bool IsInserted { get; }
        void RecordInsertion();
        Task InsertAsync(TestRunner testRunner);
    }
}

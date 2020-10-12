using System.Threading.Tasks;

namespace Models.Templates.Asbtract
{
    public interface IComplexTemplate
    {
        Task InsertAsync(TestRunner testRunner);
    }
}

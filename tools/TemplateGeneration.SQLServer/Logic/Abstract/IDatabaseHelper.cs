using System.Data;
using System.Threading.Tasks;

namespace DBConfirm.TemplateGeneration.Logic.Abstract
{
    public interface IDatabaseHelper
    {
        Task<DataTable> GetColumnsAsync(string connectionString, string schemaName, string tableName, string commandText);
    }
}

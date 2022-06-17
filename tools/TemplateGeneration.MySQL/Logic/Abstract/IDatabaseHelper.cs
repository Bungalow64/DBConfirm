using System.Data;
using System.Threading.Tasks;

namespace DBConfirm.TemplateGeneration.MySQL.Logic.Abstract
{
    public interface IDatabaseHelper
    {
        Task<DataTable> GetColumnsAsync(string connectionString, string tableName, string commandText);
    }
}

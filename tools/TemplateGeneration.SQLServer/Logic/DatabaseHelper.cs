using DBConfirm.TemplateGeneration.SQLServer.Logic.Abstract;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading.Tasks;

namespace DBConfirm.TemplateGeneration.SQLServer.Logic
{
    public class DatabaseHelper : IDatabaseHelper
    {
        public async Task<DataTable> GetColumnsAsync(string connectionString, string schemaName, string tableName, string commandText)
        {
            using SqlConnection sqlConnection = new SqlConnection(connectionString);
            await sqlConnection.OpenAsync();
            using DataSet ds = new DataSet
            {
                Locale = CultureInfo.InvariantCulture
            };

            using (SqlCommand command = new SqlCommand(commandText, sqlConnection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SqlParameter("SchemaName", schemaName));
                command.Parameters.Add(new SqlParameter("TableName", tableName));

                using SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(ds);
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }

            await sqlConnection.CloseAsync();

            return null;
        }
    }
}

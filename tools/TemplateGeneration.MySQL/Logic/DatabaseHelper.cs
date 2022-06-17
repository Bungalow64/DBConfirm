using DBConfirm.TemplateGeneration.MySQL.Logic.Abstract;
using MySql.Data.MySqlClient;
using System.Data;
using System.Globalization;
using System.Threading.Tasks;

namespace DBConfirm.TemplateGeneration.MySQL.Logic
{
    public class DatabaseHelper : IDatabaseHelper
    {
        public async Task<DataTable> GetColumnsAsync(string connectionString, string tableName, string commandText)
        {
            using MySqlConnection sqlConnection = new MySqlConnection(connectionString);
            await sqlConnection.OpenAsync();
            using DataSet ds = new DataSet
            {
                Locale = CultureInfo.InvariantCulture
            };

            using (MySqlCommand command = new MySqlCommand(commandText, sqlConnection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new MySqlParameter("TableName", tableName));

                using MySqlDataAdapter adapter = new MySqlDataAdapter(command);
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

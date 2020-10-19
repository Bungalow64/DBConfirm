using MartinCostello.SqlLocalDb;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Tools.DatabaseDeployer
{
    public class Deployer
    {
        public async Task Run(Options options)
        {
            try
            {
                await Deploy(options.InstanceName, options.Source, options.DatabaseName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        private async Task Deploy(string instanceName, string source, string databaseName)
        {
            if (!File.Exists(source))
            {
                Console.WriteLine($"Source ({source}) cannot be found.");
                return;
            }

            Regex nameTester = new Regex(@"^[a-zA-Z0-9_]*$");
            if (!nameTester.IsMatch(databaseName))
            {
                Console.WriteLine($"DatabaseName ({databaseName}) can only contain A-Z, 0-9 and _ characters.");
                return;
            }

            string sourceFile = File.ReadAllText(source);

            Console.WriteLine("Loaded source file.");

            Console.WriteLine("Connecting to instance...");
            using var localDb = new SqlLocalDbApi();
            var instance = instanceName != null ? localDb.GetOrCreateInstance(instanceName) : localDb.GetDefaultInstance();
            var manager = instance.Manage();

            if (!instance.IsRunning)
            {
                Console.WriteLine("Starting instance.");
                manager.Start();
            }

            using SqlConnection connection = instance.CreateConnection();
            await connection.OpenAsync();

            Console.WriteLine("Connected.  Checking database...");

            bool databaseExists = false;
            using (SqlCommand command = new SqlCommand(ReadFile(@"Scripts\CountDatabases.sql"), connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SqlParameter("DatabaseName", databaseName));

                object count = await command.ExecuteScalarAsync();

                databaseExists = (int)count == 1;
            }

            if (databaseExists)
            {
                Console.WriteLine("Database exists.  Dropping...");

                using (SqlCommand command = new SqlCommand(ReadFile(@"Scripts\DropDatabase.sql"), connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add(new SqlParameter("DatabaseName", databaseName));

                    await command.ExecuteNonQueryAsync();
                }

                Console.WriteLine("Dropped.");
            }

            Console.WriteLine("Creating database...");

            using (SqlCommand command = new SqlCommand(ReadFile(@"Scripts\CreateDatabase.sql"), connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SqlParameter("DatabaseName", databaseName));

                await command.ExecuteNonQueryAsync();
            }

            Console.WriteLine("Created.");

            sourceFile += Environment.NewLine;

            string[] sourceParts = sourceFile.Split($"{Environment.NewLine}GO{Environment.NewLine}", StringSplitOptions.RemoveEmptyEntries);

            foreach (string part in sourceParts)
            {
                try
                {
                    using (SqlCommand command = new SqlCommand(part, connection))
                    {
                        command.CommandType = CommandType.Text;

                        await command.ExecuteNonQueryAsync();
                    }
                } catch (Exception)
                {
                    Console.WriteLine("Error: ");
                    Console.WriteLine(part);
                    throw;
                }
            }

            await connection.CloseAsync();

            Console.WriteLine("Deployed.");


            // WHY DO THESE TABLE NOT SHOW IN SSMS?
            // WHY DOES DROPPING AND RECREATING THE DATABASE NOT ALLOW THEM TO BE RECREATED?

        }

        private static string ReadFile(string path)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"Tools.DatabaseDeployer.{path.Replace(@"\", ".")}";

            using Stream stream = assembly.GetManifestResourceStream(resourceName);
            using StreamReader reader = new StreamReader(stream);

            return reader.ReadToEnd();
        }
    }
}

using DBConfirm.TemplateGeneration.SQLServer.Logic.Abstract;
using DBConfirm.TemplateGeneration.SQLServer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DBConfirm.TemplateGeneration.SQLServer.Logic
{
    [SlowFox.InjectDependencies(typeof(Options), typeof(IFileHelper), typeof(IDatabaseHelper), typeof(IConsoleLog))]
    public partial class Generator
    {
        private const string _getColumnsScript = "DBConfirm.TemplateGeneration.SQLServer.Scripts.GetColumns.sql";

        public async Task GenerateFileAsync()
        {
            string connectionString = GetConnectionString();
            if (connectionString == null)
            {
                return;
            }

            (string schemaName, string tableName) = DetermineSchemaAndTable(_options.SchemaName, _options.TableName);

            DataTable columns = await GetColumnsAsync(connectionString, schemaName, tableName);

            List<ColumnDefinition> processedColumns = columns?.AsEnumerable().Select(p => new ColumnDefinition(p)).ToList();

            if (processedColumns == null || !processedColumns.Any())
            {
                if (_options.TableName == "*")
                {
                    _consoleLog.WriteError($"Cannot find any tables in schema: {_options.SchemaName}");
                }
                else if (_options.TableName.Contains("*"))
                {
                    _consoleLog.WriteError($"Cannot find any tables that match: {_options.SchemaName}.{_options.TableName}");
                }
                else
                {
                    _consoleLog.WriteError($"Cannot find table: {_options.SchemaName}.{_options.TableName}");
                }
                return;
            }

            IEnumerable<IGrouping<string, ColumnDefinition>> grouped = processedColumns.GroupBy(p => p.TableName);
            foreach (IGrouping<string, ColumnDefinition> group in grouped)
            {
                _consoleLog.WriteSuccess($"Found table ({schemaName}.{group.Key}) and {group.Count()} column{(group.Count() == 1 ? "" : "s")}");
            }
            foreach (IGrouping<string, ColumnDefinition> group in grouped)
            {
                GenerateFile(group, schemaName, group.Key);
            }
        }

        private (string, string) DetermineSchemaAndTable(string schemaName, string tableName)
        {
            if (schemaName == null && tableName.Contains("."))
            {
                Queue<string> parts = new Queue<string>(tableName.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries));
                schemaName = parts.Dequeue();
                tableName = string.Join(".", parts);
            }
            else
            {
                schemaName ??= "dbo";
            }

            schemaName = schemaName.Trim('[').Trim(']');
            if (tableName.Contains("*"))
            {
                tableName = tableName.Replace("*", "%");
            }
            else
            {
                tableName = tableName.Trim('[').Trim(']');
            }

            return (schemaName, tableName);
        }

        private void GenerateFile(IEnumerable<ColumnDefinition> processedColumns, string schemaName, string tableName)
        {
            (string classDefinition, string className) = GenerateClass(processedColumns, schemaName, tableName);

            if (_options.DryRun)
            {
                _consoleLog.WriteSuccess("Class definition generated.  Outputting...");
                _consoleLog.WriteSuccess(classDefinition);
            }
            else
            {
                _consoleLog.WriteSuccess("Class definition generated.  Saving...");

                string target = (_options.Destination ?? _fileHelper.GetCurrentDirectory()).TrimEnd('\\').TrimEnd('"').Trim() + $@"\{className}.cs";

                if (_fileHelper.Exists(target) && !_options.Overwrite)
                {
                    _consoleLog.WriteError($"The file ({target}) already exists");
                    return;
                }

                _fileHelper.WriteAllText(target, classDefinition);

                _consoleLog.WriteSuccess($"File written to {target}");
            }
        }

        private string GetConnectionString()
        {
            if (string.IsNullOrWhiteSpace(_options.ConnectionString) && string.IsNullOrWhiteSpace(_options.DatabaseName))
            {
                _consoleLog.WriteError("Either -c, --connectionString or -d, --databaseName needs to be supplied");
                return null;
            }

            string connectionString = _options.ConnectionString ?? $"SERVER=(local);DATABASE={_options.DatabaseName};Integrated Security=true;Connection Timeout=30;";

            _consoleLog.WriteSuccess($"Using connection: {connectionString}");

            return connectionString;
        }

        private async Task<DataTable> GetColumnsAsync(string connectionString, string schemaName, string tableName)
        {
            return await _databaseHelper.GetColumnsAsync(connectionString, schemaName, tableName, await ReadResource(_getColumnsScript));
        }

        private async Task<string> ReadResource(string resourceName)
        {
            using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
            using StreamReader reader = new StreamReader(stream);

            return await reader.ReadToEndAsync();
        }

        private (string, string) GenerateClass(IEnumerable<ColumnDefinition> processedColumns, string schemaName, string tableName)
        {
            ColumnDefinition identityColumn = processedColumns.FirstOrDefault(p => p.IsIdentity);

            string className = $"{tableName.Replace(' ', '_')}Template";

            List<string> usings = new List<string>
            {
                "using DBConfirm.Core.Data;",
                "using DBConfirm.Core.Templates;"
            };

            if (processedColumns.Any(p => p.TypeRequiresSystem))
            {
                usings.Insert(0, "using System;");
            }

            if (processedColumns.Any(p => p.ReferencesIdentity))
            {
                usings.Add("using DBConfirm.Core.Templates.Abstract;");
            }

            if (processedColumns.Any(p => p.RequiredPlaceholder))
            {
                usings.Add("using DBConfirm.Core.Templates.Placeholders;");
            }

            string output = $@"{string.Join($"{Environment.NewLine}", usings)}

namespace {_options.Namespace ?? "DBConfirm.Templates"}
{{
    public class {className} : Base{(identityColumn != null ? "Identity" : "Simple")}Template<{className}>
    {{
        public override string TableName => ""[{schemaName}].[{tableName}]"";
        {(identityColumn != null ? @$"{Environment.NewLine}        public override string IdentityColumnName => ""{identityColumn.ColumnName}"";{Environment.NewLine}" : "")}
        public override DataSetRow DefaultData => new DataSetRow
        {{
            {string.Join($",{Environment.NewLine}            ", processedColumns.Where(p => p.RequiresDefaultData).Select(p => p.ToDefaultData()))}
        }};

{string.Join($"{Environment.NewLine}", processedColumns.Select(p => p.ToFluentRow("        ", $"{className}")))}
    }}
}}";

            return (output, className);
        }
    }
}

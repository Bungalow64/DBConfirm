using CommandLine;

namespace DBConfirm.TemplateGeneration
{
    public class Options
    {
        [Option('d', "databaseName", Required = false, HelpText = "The name of the database to use")]
        public string DatabaseName { get; set; }

        [Option('c', "connectionString", Required = false, HelpText = "The connection string to use")]
        public string ConnectionString { get; set; }

        [Option('t', "tableName", Required = true, HelpText = "The name of the table to process.  The name can contain wildcard characters (*) to match multiple tables within the same schema")]
        public string TableName { get; set; }

        [Option('s', "schemaName", Required = false, HelpText = "The schema of the table to process.  Defaults to dbo")]
        public string SchemaName { get; set; }

        [Option('n', "namespace", Required = false, HelpText = "The namespace to use for the generated class (defaults to DBConfirm.Templates)")]
        public string Namespace { get; set; }

        [Option("dry-run", Required = false, HelpText = "Outputs the generated file to the console instead of creating a file")]
        public bool DryRun { get; set; }

        [Option("destination", Required = false, HelpText = "The path to where the file is to be saved.  Defaults to the current location")]
        public string Destination { get; set; }

        [Option('o', "overwrite", Required = false, HelpText = "Sets whether the target file can be overwritten if it already exists.  Defaults to false")]
        public bool Overwrite { get; set; }
    }
}

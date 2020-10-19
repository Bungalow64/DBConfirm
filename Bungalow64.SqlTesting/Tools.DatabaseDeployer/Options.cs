using CommandLine;

namespace Tools.DatabaseDeployer
{
    public class Options
    {
        [Option('i', "instanceName", Required = false, HelpText = "The name of the instance to use, otherwise the default instance is used.")]
        public string InstanceName { get; set; }

        [Option('d', "databaseName", Required = true, HelpText = "The name of the database to deploy to.  If the database exists, it will be dropped and recreated.")]
        public string DatabaseName { get; set; }

        [Option('s', "source", Required = true, HelpText = "The path to the source file for the database.  Must be a single .sql file.")]
        public string Source { get; set; }
    }
}

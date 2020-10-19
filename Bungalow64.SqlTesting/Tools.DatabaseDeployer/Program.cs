using CommandLine;
using System;
using System.Threading.Tasks;

namespace Tools.DatabaseDeployer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting up Tools.DatabaseDeployer...");

            await Parser.Default.ParseArguments<Options>(args)
                .WithParsedAsync(new Deployer().Run);

            Console.WriteLine("Done.");
        }
    }
}

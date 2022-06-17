using System;
using System.Threading.Tasks;
using CommandLine;
using DBConfirm.TemplateGeneration.MySQL.Logic;
using static DBConfirm.TemplateGeneration.MySQL.Logic.OutputHelper;

namespace DBConfirm.TemplateGeneration.MySQL
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Parser.Default.ParseArguments<Options>(args)
                   .WithParsedAsync(async o =>
                   {
                       try
                       {
                           await new Generator(o, new FileHelper(), new DatabaseHelper(), new ConsoleLog()).GenerateFileAsync();
                       }
                       catch (Exception ex)
                       {
                           WriteError("There has been an error");
                           WriteException(ex);
                       }
                   });
        }
    }
}

using System;
using System.Threading.Tasks;
using CommandLine;
using SQLConfirm.Tools.TemplateGeneration.SQLServer.Logic;
using static SQLConfirm.Tools.TemplateGeneration.SQLServer.Logic.OutputHelper;

namespace SQLConfirm.Tools.TemplateGeneration.SQLServer
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
                           await new Generator(o).GenerateFileAsync();
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

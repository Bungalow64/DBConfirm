using DBConfirm.TemplateGeneration.MySQL.Logic.Abstract;

namespace DBConfirm.TemplateGeneration.MySQL.Logic
{
    public class ConsoleLog : IConsoleLog
    {
        public void WriteError(string text) => OutputHelper.WriteError(text);

        public void WriteSuccess(string text) => OutputHelper.WriteSuccess(text);
    }
}

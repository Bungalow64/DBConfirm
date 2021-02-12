using DBConfirm.TemplateGeneration.Logic.Abstract;

namespace DBConfirm.TemplateGeneration.Logic
{
    public class ConsoleLog : IConsoleLog
    {
        public void WriteError(string text) => OutputHelper.WriteError(text);

        public void WriteSuccess(string text) => OutputHelper.WriteSuccess(text);
    }
}

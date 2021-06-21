namespace DBConfirm.TemplateGeneration.Logic.Abstract
{
    public interface IConsoleLog
    {
        void WriteError(string text);
        void WriteSuccess(string text);
    }
}

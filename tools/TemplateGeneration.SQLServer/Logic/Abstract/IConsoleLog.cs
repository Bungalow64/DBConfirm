namespace DBConfirm.TemplateGeneration.SQLServer.Logic.Abstract
{
    public interface IConsoleLog
    {
        void WriteError(string text);
        void WriteSuccess(string text);
    }
}

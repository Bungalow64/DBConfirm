namespace DBConfirm.TemplateGeneration.SQLServer.Logic.Abstract
{
    public interface IFileHelper
    {
        string GetCurrentDirectory();
        bool Exists(string path);
        void WriteAllText(string path, string contents);
    }
}

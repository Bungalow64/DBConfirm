namespace DBConfirm.TemplateGeneration.Logic.Abstract
{
    public interface IFileHelper
    {
        string GetCurrentDirectory();
        bool Exists(string path);
        void WriteAllText(string path, string contents);
    }
}

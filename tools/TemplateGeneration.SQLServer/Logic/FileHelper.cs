using DBConfirm.TemplateGeneration.Logic.Abstract;
using System.IO;

namespace DBConfirm.TemplateGeneration.Logic
{
    public class FileHelper : IFileHelper
    {
        public bool Exists(string path) => File.Exists(path);

        public void WriteAllText(string path, string contents)
        {
            var directoryPath = Path.GetDirectoryName(path);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            File.WriteAllText(path, contents);
        }

        public string GetCurrentDirectory() => Directory.GetCurrentDirectory();
    }
}

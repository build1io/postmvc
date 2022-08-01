using System.IO;

namespace Build1.PostMVC.Extensions.Unity.Modules.Logging
{
    public sealed class LogFile
    {
        public readonly string path;

        internal LogFile(string path)
        {
            this.path = path;
        }
        
        public string GetNameWithoutExtension()
        {
            return Path.GetFileNameWithoutExtension(path);
        }
        
        public string GetName()
        {
            return Path.GetFileName(path);
        }

        public string ReadContent()
        {
            return File.ReadAllText(path);
        }
    }
}
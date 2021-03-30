namespace Build1.PostMVC.Extensions.Unity.Modules.App
{
    public interface IAppController
    {
        string Version     { get; }
        int    BuildNumber { get; }

        bool IsPaused  { get; }
        bool IsFocused { get; }

        void OpenUrl(string url);
        void MailTo(string email, string subject, string body);
        void CopyToClipboard(string content);
        
        void Restart();
    }
}
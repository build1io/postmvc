namespace Build1.PostMVC.Extensions.Unity.Modules.App
{
    public interface IAppController
    {
        string PersistentDataPath { get; }
        string Version            { get; }

        bool IsPaused  { get; }
        bool IsFocused { get; }

        void Restart();
    }
}
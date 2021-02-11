namespace Build1.PostMVC.Extensions.Unity.Modules.App
{
    public interface IAppController
    {
        string Version     { get; }
        int    BuildNumber { get; }
        bool   IsPaused    { get; }

        void Restart();
    }
}
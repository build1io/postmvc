namespace Build1.PostMVC.Extensions.Unity.Mediation.Api
{
    internal interface IUnityViewEventProcessor
    {
        void ProcessStart(IUnityView view);
        void ProcessOnEnable(IUnityView view);
        void ProcessOnDisable(IUnityView view);
    }
}
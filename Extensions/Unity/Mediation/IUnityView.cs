using Build1.PostMVC.Extensions.MVCS.Mediation;

namespace Build1.PostMVC.Extensions.Unity.Mediation
{
    public interface IUnityView : IView
    {
        bool Initialized { get; }
        bool Enabled     { get; }
    }
}
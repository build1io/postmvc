using Build1.PostMVC.Core.MVCS.Events;

namespace Build1.PostMVC.Core.MVCS.Mediation
{
    public interface IViewWithDispatcher : IView, IEventDispatcher
    {
    }
}
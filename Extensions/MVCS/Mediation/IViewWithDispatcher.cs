using Build1.PostMVC.Extensions.MVCS.Events;

namespace Build1.PostMVC.Extensions.MVCS.Mediation
{
    public interface IViewWithDispatcher : IView, IEventDispatcher
    {
    }
}
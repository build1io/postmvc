using Build1.PostMVC.Core.Extensions.MVCS.Events;

namespace Build1.PostMVC.Core.Extensions.MVCS.Mediation
{
    public interface IViewWithDispatcher : IView, IEventDispatcher
    {
    }
}
using Build1.PostMVC.Utils.Pooling;

namespace Build1.PostMVC.Extensions.MVCS.Events.Impl
{
    internal interface IEventMapInfo : IPoolable
    {
        public abstract void Unbind();
        public abstract bool Match(IEventDispatcher dispatcher, EventBase @event, object listener);
    }
}
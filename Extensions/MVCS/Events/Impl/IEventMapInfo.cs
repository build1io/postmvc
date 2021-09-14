namespace Build1.PostMVC.Extensions.MVCS.Events.Impl
{
    internal interface IEventMapInfo
    {
        public abstract void Unbind();
        public abstract bool Match(IEventDispatcher dispatcher, EventBase @event, object listener);
    }
}
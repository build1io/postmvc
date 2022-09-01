namespace Build1.PostMVC.Core.MVCS.Events.Impl.Map
{
    public interface IEventMapInfo
    {
        IEventMapInfo Unbind();
        bool          Match(IEventDispatcher dispatcher, EventBase @event, object listener);
    }
}
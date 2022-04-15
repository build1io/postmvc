namespace Build1.PostMVC.Extensions.MVCS.Events.Impl
{
    internal interface IEventMapInfo
    {
        IEventMapInfo Unbind();
        bool          Match(IEventDispatcher dispatcher, EventBase @event, object listener);
    }
}
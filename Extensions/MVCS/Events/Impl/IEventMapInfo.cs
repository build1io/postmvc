namespace Build1.PostMVC.Extensions.MVCS.Events.Impl
{
    internal interface IEventMapInfo
    {
        void Unbind();
        bool Match(IEventDispatcher dispatcher, EventBase @event, object listener);
    }
}
namespace Build1.PostMVC.Extensions.MVCS.Events.Impl
{
    internal abstract class EventMapInfoBase
    {
        protected readonly IEventDispatcher dispatcher;

        protected EventMapInfoBase(IEventDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        public abstract void Unbind();
        public abstract bool Match(IEventDispatcher dispatcher, EventBase @event, object listener);
    }
}
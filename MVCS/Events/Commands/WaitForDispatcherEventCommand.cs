using Build1.PostMVC.Core.MVCS.Commands;

namespace Build1.PostMVC.Core.MVCS.Events.Commands
{
    [Poolable]
    public sealed class WaitForDispatcherEventCommand : Command<IEventDispatcher, Event>
    {
        private IEventDispatcher _dispatcher;
        
        public override void Execute(IEventDispatcher dispatcher, Event @event)
        {
            Retain();

            _dispatcher = dispatcher;
            _dispatcher.AddListenerOnce(@event, OnListened);
        }

        private void OnListened()
        {
            _dispatcher = null;
            Release();
        }
    }
}
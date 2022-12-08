using Build1.PostMVC.Core.MVCS.Commands;
using Build1.PostMVC.Core.MVCS.Injection;

namespace Build1.PostMVC.Core.MVCS.Events.Commands
{
    [Poolable]
    public sealed class WaitForEventCommand : Command<Event>
    {
        [Inject] public IEventDispatcher Dispatcher { get; set; }
        
        public override void Execute(Event @event)
        {
            Retain();
            Dispatcher.AddListenerOnce(@event, Release);
        }
    }
}
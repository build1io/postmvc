using Build1.PostMVC.Extensions.MVCS.Events.Impl;

namespace Build1.PostMVC.Extensions.Unity.Events.Mapping.Impl
{
    internal sealed class EventMapProvider : MVCS.Events.Mapping.Impl.EventMapProvider
    {
        protected override MVCS.Events.Mapping.IEventMapper CreateEventMapper()
        {
            return new EventMapper((EventDispatcherWithCommandProcessing)Dispatcher);
        }
    }
}
using Build1.PostMVC.Core.MVCS.Injection;

namespace Build1.PostMVC.Core.MVCS.Events.Impl.Map
{
    public sealed class EventMapProvider : EventMapProviderBase<IEventMapCore>
    {
        [Inject] public IEventDispatcher Dispatcher { get; set; }
        [Inject] public IEventBus        EventBus   { get; set; }

        protected override IEventMapCore CreateMap()
        {
            return new EventMap(Dispatcher, EventBus, _infoPools);
        }

        protected override void OnMapReturn(IEventMapCore map)
        {
            map.UnmapAll();
        }
    }
}
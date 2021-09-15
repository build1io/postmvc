using Build1.PostMVC.Utils.Pooling;

namespace Build1.PostMVC.Extensions.MVCS.Events.Impl
{
    internal interface IEventBusEventInfo : IPoolable
    {
        void Dispatch();
    }
}
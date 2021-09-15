namespace Build1.PostMVC.Extensions.MVCS.Events
{
    public interface IEventBus
    {
        bool HasEvents { get; }

        void Add(Event @event);
        void Add<T1>(Event<T1> @event, T1 param01);
        void Add<T1, T2>(Event<T1, T2> @event, T1 param01, T2 param02);
        void Add<T1, T2, T3>(Event<T1, T2, T3> @event, T1 param01, T2 param02, T3 param03);
        
        void Dispatch();
    }
}
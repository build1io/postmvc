namespace Build1.PostMVC.Extensions.MVCS.Events
{
    public interface IEventMap : IEventMapper
    {
        /*
         * Dispatching.
         */
        
        void Dispatch(Event @event);
        void Dispatch<T1>(Event<T1> @event, T1 param01);
        void Dispatch<T1, T2>(Event<T1, T2> @event, T1 param01, T2 param02);
        void Dispatch<T1, T2, T3>(Event<T1, T2, T3> @event, T1 param01, T2 param02, T3 param03);
    }
}
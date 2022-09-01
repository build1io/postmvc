namespace Build1.PostMVC.Core.Extensions.MVCS.Events.Impl
{
    public abstract class EventBase
    {
        private static int Id;

        private readonly int _id;

        protected EventBase()
        {
            _id = ++Id;
        }

        public override int  GetHashCode()         { return _id; }
        public override bool Equals(object obj)    { return Equals(obj as EventBase); }
        public          bool Equals(EventBase obj) { return obj != null && obj._id == _id; }
    }
}
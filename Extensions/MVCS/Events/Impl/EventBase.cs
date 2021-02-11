namespace Build1.PostMVC.Extensions.MVCS.Events.Impl
{
    public abstract class EventBase
    {
        private static int Id;
        
        private readonly int id;
        
        protected EventBase()
        {
            id = Id++;
        }

        public override bool Equals(object obj)
        {
            if (obj is EventBase eventBase)
                return id == eventBase.id;
            return false;
        }

        public override int GetHashCode()
        {
            return id;
        }
    }
}